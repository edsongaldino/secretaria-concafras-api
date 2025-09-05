// Controllers/ComissoesEventoController.cs  (trechos novos/simplificados)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Comissoes;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure;
using System.Net.Mime;

namespace SecretariaConcafras.API.Controllers;

[ApiController]
[Route("api/comissoes-evento")]
[Produces(MediaTypeNames.Application.Json)]
public class ComissoesEventoController : ControllerBase
{
	private readonly ApplicationDbContext _db;
	public ComissoesEventoController(ApplicationDbContext db) => _db = db;

	// GET /api/comissoes-evento?eventoId=...&q=...&skip=0&take=100
	// Lista leve (sem usuários / sem inscritos)
	[HttpGet]
	public async Task<ActionResult<IEnumerable<ComissaoEventoListItemDto>>> ListarLeve(
		[FromQuery] Guid eventoId,
		[FromQuery] string? q,
		[FromQuery] int skip = 0,
		[FromQuery] int take = 100,
		CancellationToken ct = default)
	{
		if (eventoId == Guid.Empty) return BadRequest("eventoId é obrigatório.");

		var query = _db.Set<ComissaoEvento>()
			.AsNoTracking()
			.Where(ce => ce.EventoId == eventoId);

		if (!string.IsNullOrWhiteSpace(q))
		{
			var term = q.Trim().ToLower();
			query = query.Where(ce => EF.Functions.ILike(ce.Comissao.Nome, $"%{term}%"));
		}

		var data = await query
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
			.ToListAsync(ct);

		return Ok(data);
	}

	// (opcional) GET /api/comissoes-evento/contagens?eventoId=...
	// Devolve só as contagens para badges (sem expandir coleções)
	[HttpGet("contagens")]
	public async Task<ActionResult<IEnumerable<ComissaoEventoCountsDto>>> Contagens(
		[FromQuery] Guid eventoId,
		CancellationToken ct = default)
	{
		if (eventoId == Guid.Empty) return BadRequest("eventoId é obrigatório.");

		var data = await _db.Set<ComissaoEvento>()
			.AsNoTracking()
			.Where(ce => ce.EventoId == eventoId)
			.Select(ce => new ComissaoEventoCountsDto
			{
				ComissaoEventoId = ce.Id,
				QtdInscritosTrabalhador = ce.InscricoesTrabalhadores.Count(),
				QtdUsuarioRoles = ce.UsuarioRoles.Count()
			})
			.ToListAsync(ct);

		return Ok(data);
	}

	// (opcional) GET /api/comissoes-evento/{id}/inscricoes
	[HttpGet("{id:guid}/inscricoes")]
	public async Task<ActionResult<IEnumerable<InscricaoTrabalhadorSlimDto>>> ListarInscricoes(Guid id, CancellationToken ct)
	{
		var data = await _db.Set<InscricaoTrabalhador>()
			.AsNoTracking()
			.Where(it => it.ComissaoEventoId == id)
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
			.ToListAsync(ct);

		return Ok(data);
	}

	// (opcional) GET /api/comissoes-evento/{id}/usuarios
	[HttpGet("{id:guid}/usuarios")]
	public async Task<ActionResult<IEnumerable<UsuarioRoleSlimDto>>> ListarUsuarios(Guid id, CancellationToken ct)
	{
		var data = await _db.Set<UsuarioRole>()
			.AsNoTracking()
			.Where(ur => ur.ComissaoEventoId == id)
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
			.ToListAsync(ct);

		return Ok(data);
	}
}
