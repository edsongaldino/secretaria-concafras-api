using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Inscricoes;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Domain.Enums;
using SecretariaConcafras.Domain.Exceptions;
using SecretariaConcafras.Domain.Helpers;
using SecretariaConcafras.Infrastructure;
using System;
using System.Net;

namespace SecretariaConcafras.Application.Services.Implementations
{
    public class InscricaoService : IInscricaoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public InscricaoService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InscricaoCreateResultDto> CriarAsync(InscricaoCreateDto dto)
        {
            // 0) sanity checks (existência)
            var evento = await _context.Eventos.Where(e => e.Id == dto.EventoId).SingleOrDefaultAsync();

			if (evento is null)
                throw new InscricaoException(HttpStatusCode.BadRequest, "Evento não encontrado.",
                    new Dictionary<string, string[]> { ["EventoId"] = new[] { "Id inválido." } });

			var participante = await _context.Participantes
	            .Where(p => p.Id == dto.ParticipanteId)
	            .SingleOrDefaultAsync();

			if (participante is null)
				throw new InscricaoException(HttpStatusCode.Conflict, "Participante não encontrado.");

			// 1) validação: curso XOR comissão
			var temCurso = dto.CursoTemaAtualId.HasValue && dto.CursoTemaEspecificoId.HasValue;
            var temComissao = dto.ComissaoId.HasValue;

            // 2) duplicidade
            var jaExiste = await _context.Inscricoes
                .AnyAsync(i => i.EventoId == dto.EventoId && i.ParticipanteId == dto.ParticipanteId);

            if (jaExiste)
                throw new InscricaoException(HttpStatusCode.Conflict, "Participante já inscrito neste evento.");

            // 3) inferências
            var trabalhador = temComissao;
            var neofito = !trabalhador; // ajuste sua regra se for diferente

            var responsavelId = dto.ResponsavelFinanceiroId ?? dto.ParticipanteId;

            var valorInscricao = InscricaoHelper.DefinirValorInscricao(evento, participante);

			// 4) persiste
			var ins = new Inscricao
            {
                Id = Guid.NewGuid(),
                EventoId = dto.EventoId,
                ParticipanteId = dto.ParticipanteId,
                ResponsavelFinanceiroId = responsavelId,
                DataInscricao = DateTime.UtcNow,
                PagamentoConfirmado = false,
                ValorInscricao = valorInscricao
			};

            _context.Inscricoes.Add(ins);

            if (temComissao)
            {
                _context.Set<InscricaoTrabalhador>().Add(new InscricaoTrabalhador
                {
                    InscricaoId = ins.Id,
                    ComissaoEventoId = dto.ComissaoId!.Value,
                    Nivel = NivelTrabalhador.Voluntario
                });

            }

            if (temCurso)
            {
                _context.Set<InscricaoCurso>().Add(new InscricaoCurso { 
                    InscricaoId = ins.Id, 
                    CursoId = dto.CursoTemaAtualId.Value
                });

                _context.Set<InscricaoCurso>().Add(new InscricaoCurso
                {
                    InscricaoId = ins.Id,
                    CursoId = dto.CursoTemaEspecificoId.Value
                });

            }

            await _context.SaveChangesAsync();

            return new InscricaoCreateResultDto
            {
                Id = ins.Id,
                Trabalhador = trabalhador,
                Message = "Inscrição criada"
            };
        }

        public async Task<bool> CancelarAsync(Guid id)
        {
            var entity = await _context.Inscricoes.FindAsync(id);
            if (entity == null) return false;

            _context.Inscricoes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<InscricaoResponseDto?> ObterPorIdAsync(Guid id)
        {
            var entity = await _context.Inscricoes
                .Include(i => i.Participante)
                .Include(i => i.Evento)
                .Include(i => i.Cursos)
                .Include(i => i.InscricaoTrabalhador)
                .FirstOrDefaultAsync(i => i.Id == id);

            return entity == null ? null : _mapper.Map<InscricaoResponseDto>(entity);
        }

        public async Task<IEnumerable<InscricaoResponseDto>> ObterPorEventoAsync(Guid eventoId)
        {
            var entities = await _context.Inscricoes
                .Include(i => i.Participante)
                .Include(i => i.Evento)
                .Include(i => i.Cursos)
                .Include(i => i.InscricaoTrabalhador)
                .Where(i => i.EventoId == eventoId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InscricaoResponseDto>>(entities);
        }

        public async Task<IEnumerable<InscricaoResponseDto>> ObterPorParticipanteAsync(Guid participanteId)
        {
            var entities = await _context.Inscricoes
                .Include(i => i.Participante)
                .Include(i => i.Evento)
                .Include(i => i.Cursos)
                .Include(i => i.InscricaoTrabalhador)
                .Where(i => i.ParticipanteId == participanteId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InscricaoResponseDto>>(entities);
        }

        public async Task<List<ListaInscricoesDTO>> ListaInscricoesAsync(Guid eventoId, Guid responsavelId)
        {
            var inscricoes = await _context.Inscricoes
                .AsNoTracking()
                .Where(x => x.EventoId == eventoId && x.ResponsavelFinanceiroId == responsavelId)
                .Include(x => x.Evento)
                .Include(x => x.Participante)
                .Include(x => x.Cursos).ThenInclude(ic => ic.Curso)
                .Include(x => x.InscricaoTrabalhador)
                .ToListAsync();

            return inscricoes.Select(i => new ListaInscricoesDTO
            {
                InscricaoId = i.Id,
                EventoId = i.EventoId,
                EventoTitulo = i.Evento.Titulo,
                DataInscricao = i.DataInscricao,
                TemaAtual = i.Cursos.FirstOrDefault(c => c.Curso.Bloco == BlocoCurso.TemaAtual)?.Curso.Titulo ?? "—",
                TemaEspecifico = i.Cursos.FirstOrDefault(c => c.Curso.Bloco == BlocoCurso.TemaEspecifico)?.Curso.Titulo ?? "—",
                Trabalhador = i.InscricaoTrabalhador != null,
                PagamentoStatus = "Pendente",
				ValorInscricao = i.ValorInscricao,
                ParticipanteId = i.ParticipanteId,
                ParticipanteNome = i.Participante.Nome,
                ParticipanteDataNascimento = i.Participante.DataNascimento,
                ParticipanteIdade = DataHelper.CalcularIdadePelaData(i.Participante.DataNascimento, i.Evento.DataInicio)
            }).ToList();
        }

    }
}
