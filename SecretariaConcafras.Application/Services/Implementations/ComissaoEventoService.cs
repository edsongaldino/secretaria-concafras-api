using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Comissoes;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure;
using System.Linq.Expressions;

namespace SecretariaConcafras.Application.Services
{
    public class ComissaoEventoService : IComissaoEventoService
    {
        private readonly ApplicationDbContext _db;
        public ComissaoEventoService(ApplicationDbContext db) => _db = db;

        public async Task CriarParaEventoAsync(ComissoesParaEventoCreateDto dto)
        {
            if (dto.EventoId == Guid.Empty) throw new ArgumentException("EventoId é obrigatório.", nameof(dto.EventoId));
            if (dto.ComissaoIds is null || !dto.ComissaoIds.Any()) throw new ArgumentException("Informe ao menos uma comissão.", nameof(dto.ComissaoIds));

            var existentes = await _db.Set<ComissaoEvento>()
                .Where(ce => ce.EventoId == dto.EventoId && dto.ComissaoIds.Contains(ce.ComissaoId))
                .Select(ce => ce.ComissaoId)
                .ToListAsync();

            var coordMap = (dto.Coordenadores ?? Enumerable.Empty<CoordenadorVinculoDto>())
                .GroupBy(x => x.ComissaoId)
                .ToDictionary(g => g.Key, g => g.First().UsuarioId);

            foreach (var comissaoId in dto.ComissaoIds.Distinct())
            {
                if (existentes.Contains(comissaoId)) continue;

                var ce = new ComissaoEvento
                {
                    Id = Guid.NewGuid(),
                    EventoId = dto.EventoId,
                    ComissaoId = comissaoId,
                    CoordenadorUsuarioId = coordMap.TryGetValue(comissaoId, out var uid) ? uid : (Guid?)null
                };
                _db.Add(ce);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<ComissaoEventoDto?> ObterPorIdAsync(Guid id)
        {
            return await _db.Set<ComissaoEvento>()
                .AsNoTracking()
                .Where(ce => ce.Id == id)
                .Select(ProjComissaoEventoDto())
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ComissaoEventoDto>> ObterPorEventoAsync(Guid eventoId)
        {
            return await _db.Set<ComissaoEvento>()
                .AsNoTracking()
                .Where(ce => ce.EventoId == eventoId)
                .OrderBy(ce => ce.Comissao.Nome)
                .Select(ProjComissaoEventoDto())
                .ToListAsync();
        }

        public async Task AtualizarAsync(Guid id, ComissaoEventoUpdateDto dto)
        {
            var ce = await _db.Set<ComissaoEvento>().FirstOrDefaultAsync(x => x.Id == id);
            if (ce is null) throw new KeyNotFoundException("Comissão do evento não encontrada.");

            ce.Observacoes = dto.Observacoes;

            if (dto.CoordenadorUsuarioId.HasValue)
            {
                var uid = dto.CoordenadorUsuarioId.Value;
                if (uid != Guid.Empty)
                {
                    var usuarioExiste = await _db.Set<Usuario>().AnyAsync(u => u.Id == uid);
                    if (!usuarioExiste) throw new InvalidOperationException("Usuário coordenador não encontrado.");
                    ce.CoordenadorUsuarioId = uid;
                }
                else
                {
                    ce.CoordenadorUsuarioId = null;
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task AtualizarCoordenadorAsync(Guid id, Guid? usuarioId)
        {
            var ce = await _db.Set<ComissaoEvento>().FirstOrDefaultAsync(x => x.Id == id);
            if (ce is null) throw new KeyNotFoundException("Comissão do evento não encontrada.");

            if (usuarioId.HasValue && usuarioId.Value != Guid.Empty)
            {
                var ok = await _db.Set<Usuario>().AnyAsync(u => u.Id == usuarioId.Value);
                if (!ok) throw new InvalidOperationException("Usuário coordenador não encontrado.");
                ce.CoordenadorUsuarioId = usuarioId.Value;
            }
            else
            {
                ce.CoordenadorUsuarioId = null;
            }

            await _db.SaveChangesAsync();
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            var ce = await _db.Set<ComissaoEvento>()
                .Include(x => x.InscricoesTrabalhadores) // checar dependências
                .FirstOrDefaultAsync(x => x.Id == id);

            if (ce is null) return false;

            if (ce.InscricoesTrabalhadores?.Any() == true)
                throw new InvalidOperationException("Não é possível remover: existem trabalhadores vinculados.");

            _db.Remove(ce);
            await _db.SaveChangesAsync();
            return true;
        }

        // ---------- Projection helper ----------
        private static Expression<Func<ComissaoEvento, ComissaoEventoDto>> ProjComissaoEventoDto()
        {
            return ce => new ComissaoEventoDto
            {
                Id = ce.Id,
                EventoId = ce.EventoId,
                ComissaoId = ce.ComissaoId,
                Comissao = new ComissaoDto
                {
                    Id = ce.Comissao.Id,
                    Nome = ce.Comissao.Nome,
                    Slug = ce.Comissao.Slug,
                    Ativa = ce.Comissao.Ativa
                },
                CoordenadorUsuarioId = ce.CoordenadorUsuarioId,
                CoordenadorUsuario = ce.CoordenadorUsuario == null ? null : new UsuarioSlimDto
                {
                    Id = ce.CoordenadorUsuario.Id,
                    Nome = ce.CoordenadorUsuario.Nome,
                    Email = ce.CoordenadorUsuario.Email
                },
                Observacoes = ce.Observacoes,
                InscricoesTrabalhadores = ce.InscricoesTrabalhadores
                    .Select(it => new InscricaoTrabalhadorSlimDto
                    {
                        Id = it.Id,
                        InscricaoId = it.InscricaoId,
                        ComissaoEventoId = it.ComissaoEventoId,
                        Nivel = (int)it.Nivel, // troque para string se enum for serializado como string
                        Inscricao = new InscricaoSlimDto
                        {
                            Id = it.Inscricao.Id,
                            ParticipanteId = it.Inscricao.ParticipanteId,
                            ParticipanteNome = it.Inscricao.Participante.Nome
                        }
                    })
                    .ToList(),
                UsuarioRoles = ce.UsuarioRoles
                    .Select(ur => new UsuarioRoleSlimDto
                    {
                        Id = ur.Id,
                        UsuarioId = ur.UsuarioId,
                        Role = (int)ur.Role, // troque para string se precisar
                        EventoId = ur.EventoId,
                        ComissaoEventoId = ur.ComissaoEventoId,
                        Usuario = new UsuarioSlimDto
                        {
                            Id = ur.Usuario.Id,
                            Nome = ur.Usuario.Nome,
                            Email = ur.Usuario.Email
                        }
                    })
                    .ToList()
            };
        }
    }
}
