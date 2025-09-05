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

		public async Task<IEnumerable<ComissaoEventoListItemDto>> ListarAsync(Guid eventoId, string? q = null, int skip = 0, int take = 100)
		{
			var query = _db.Set<ComissaoEvento>()
				.AsNoTracking()
				.Where(ce => ce.EventoId == eventoId);

			if (!string.IsNullOrWhiteSpace(q))
			{
				var term = q.Trim();
				query = query.Where(ce => EF.Functions.ILike(ce.Comissao.Nome, $"%{term}%"));
			}

			return await query
				.OrderBy(ce => ce.Comissao.Nome)
				.Skip(skip).Take(take)
				.Select(ce => new ComissaoEventoListItemDto
				{
					Id = ce.Id,
					EventoId = ce.EventoId,
					ComissaoId = ce.ComissaoId,
					ComissaoNome = ce.Comissao.Nome,
					Observacoes = ce.Observacoes
				})
				.ToListAsync();
		}

		public async Task<IEnumerable<ComissaoEventoCountsDto>> ContagensAsync(Guid eventoId)
		{
			return await _db.Set<ComissaoEvento>()
				.AsNoTracking()
				.Where(ce => ce.EventoId == eventoId)
				.Select(ce => new ComissaoEventoCountsDto
				{
					ComissaoEventoId = ce.Id,
					QtdInscritosTrabalhador = ce.InscricoesTrabalhadores.Count(),
					QtdUsuarioRoles = ce.UsuarioRoles.Count()
				})
				.ToListAsync();
		}

		public async Task<IEnumerable<InscricaoTrabalhadorSlimDto>> ListarInscricoesAsync(Guid comissaoEventoId)
		{
			return await _db.Set<InscricaoTrabalhador>()
				.AsNoTracking()
				.Where(it => it.ComissaoEventoId == comissaoEventoId)
				.Select(it => new InscricaoTrabalhadorSlimDto
				{
					Id = it.Id,
					InscricaoId = it.InscricaoId,
					ComissaoEventoId = it.ComissaoEventoId,
					Nivel = (int)it.Nivel,
					Inscricao = new InscricaoSlimDto
					{
						Id = it.Inscricao.Id,
						ParticipanteId = it.Inscricao.ParticipanteId,
						ParticipanteNome = it.Inscricao.Participante.Nome
					}
				})
				.ToListAsync();
		}

		public async Task<IEnumerable<UsuarioRoleSlimDto>> ListarUsuariosAsync(Guid comissaoEventoId)
		{
			return await _db.Set<UsuarioRole>()
				.AsNoTracking()
				.Where(ur => ur.ComissaoEventoId == comissaoEventoId)
				.Select(ur => new UsuarioRoleSlimDto
				{
					Id = ur.Id,
					UsuarioId = ur.UsuarioId,
					Role = (int)ur.Role,
					EventoId = ur.EventoId,
					ComissaoEventoId = ur.ComissaoEventoId,
					Usuario = new UsuarioSlimDto
					{
						Id = ur.Usuario.Id,
						Nome = ur.Usuario.Nome,
						Email = ur.Usuario.Email
					}
				})
				.ToListAsync();
		}
	}
}
