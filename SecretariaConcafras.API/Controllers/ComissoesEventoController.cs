using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Domain.Entities; // ComissaoEvento, Comissao, Usuario, InscricaoTrabalhador, Inscricao, UsuarioRole
using SecretariaConcafras.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace SecretariaConcafras.API.Controllers;

[ApiController]
[Route("api/comissoes-evento")]
[Produces(MediaTypeNames.Application.Json)]
public class ComissoesEventoController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public ComissoesEventoController(ApplicationDbContext db) => _db = db;

    // GET /api/comissoes-evento?eventoId=...
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComissaoEventoDto>>> ListarPorEvento([FromQuery] Guid eventoId, CancellationToken ct)
    {
        if (eventoId == Guid.Empty) return BadRequest("eventoId é obrigatório.");

        var data = await _db.Set<ComissaoEvento>()
            .AsNoTracking()
            .Where(ce => ce.EventoId == eventoId)
            .OrderBy(ce => ce.Comissao.Nome)
            .Select(ce => new ComissaoEventoDto(
                ce.Id,
                ce.EventoId,
                ce.ComissaoId,
                new ComissaoDto(ce.Comissao.Id, ce.Comissao.Nome, ce.Comissao.Slug, ce.Comissao.Ativa),
                ce.CoordenadorUsuarioId,
                ce.CoordenadorUsuario != null ? new UsuarioSlimDto(ce.CoordenadorUsuario.Id, ce.CoordenadorUsuario.Nome, ce.CoordenadorUsuario.Email) : null,
                ce.Observacoes,
                ce.InscricoesTrabalhadores.Select(it => new InscricaoTrabalhadorDto(
                    it.Id,
                    it.InscricaoId,
                    it.ComissaoEventoId,
                    it.Nivel,
                    new InscricaoSlimDto(
                        it.Inscricao.Id,
                        it.Inscricao.ParticipanteId,
                        it.Inscricao.Participante.Nome
                    )
                )).ToList(),
                ce.UsuarioRoles.Select(ur => new UsuarioRoleDto(
                    ur.Id,
                    ur.UsuarioId,
                    ur.Role,
                    ur.EventoId,
                    ur.ComissaoEventoId,
                    new UsuarioSlimDto(ur.Usuario.Id, ur.Usuario.Nome, ur.Usuario.Email)
                )).ToList()
            ))
            .ToListAsync(ct);

        return Ok(data);
    }

    // GET /api/comissoes-evento/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ComissaoEventoDto>> Obter(Guid id, CancellationToken ct)
    {
        var ce = await _db.Set<ComissaoEvento>()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(ce => new ComissaoEventoDto(
                ce.Id,
                ce.EventoId,
                ce.ComissaoId,
                new ComissaoDto(ce.Comissao.Id, ce.Comissao.Nome, ce.Comissao.Slug, ce.Comissao.Ativa),
                ce.CoordenadorUsuarioId,
                ce.CoordenadorUsuario != null ? new UsuarioSlimDto(ce.CoordenadorUsuario.Id, ce.CoordenadorUsuario.Nome, ce.CoordenadorUsuario.Email) : null,
                ce.Observacoes,
                ce.InscricoesTrabalhadores.Select(it => new InscricaoTrabalhadorDto(
                    it.Id, it.InscricaoId, it.ComissaoEventoId, it.Nivel,
                    new InscricaoSlimDto(it.Inscricao.Id, it.Inscricao.ParticipanteId, it.Inscricao.Participante.Nome)
                )).ToList(),
                ce.UsuarioRoles.Select(ur => new UsuarioRoleDto(
                    ur.Id, ur.UsuarioId, ur.Role, ur.EventoId, ur.ComissaoEventoId,
                    new UsuarioSlimDto(ur.Usuario.Id, ur.Usuario.Nome, ur.Usuario.Email)
                )).ToList()
            ))
            .FirstOrDefaultAsync(ct);

        if (ce is null) return NotFound();
        return Ok(ce);
    }

    // POST /api/comissoes-evento
    // Cria vínculos de comissões do catálogo para um evento (uma ou várias)
    [HttpPost]
    public async Task<IActionResult> CriarParaEvento([FromBody] CriarComissoesParaEventoRequest body, CancellationToken ct)
    {
        if (body.EventoId == Guid.Empty) return BadRequest("EventoId é obrigatório.");
        if (body.ComissaoIds is null || !body.ComissaoIds.Any()) return BadRequest("Informe ao menos uma comissão.");

        // carrega existentes para evitar duplicar
        var existentes = await _db.Set<ComissaoEvento>()
            .Where(ce => ce.EventoId == body.EventoId && body.ComissaoIds.Contains(ce.ComissaoId))
            .Select(ce => ce.ComissaoId)
            .ToListAsync(ct);

        var coordMap = (body.Coordenadores ?? Array.Empty<CoordenadorVinculo>()).ToDictionary(x => x.ComissaoId, x => x.UsuarioId);

        foreach (var comissaoId in body.ComissaoIds.Distinct())
        {
            if (existentes.Contains(comissaoId)) continue;

            var ce = new ComissaoEvento
            {
                Id = Guid.NewGuid(),
                EventoId = body.EventoId,
                ComissaoId = comissaoId,
                CoordenadorUsuarioId = coordMap.TryGetValue(comissaoId, out var uId) ? uId : null
            };

            _db.Add(ce);
        }

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    // PUT /api/comissoes-evento/{id}/coordenador
    [HttpPut("{id:guid}/coordenador")]
    public async Task<IActionResult> AtualizarCoordenador(Guid id, [FromBody] AtualizarCoordenadorRequest body, CancellationToken ct)
    {
        var ce = await _db.Set<ComissaoEvento>().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (ce is null) return NotFound();

        // opcionalmente validar se o usuário existe quando informado
        if (body.UsuarioId is { } uid && uid != Guid.Empty)
        {
            var exists = await _db.Set<Usuario>().AnyAsync(u => u.Id == uid, ct);
            if (!exists) return BadRequest("Usuário não encontrado.");
            ce.CoordenadorUsuarioId = uid;
        }
        else
        {
            ce.CoordenadorUsuarioId = null;
        }

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    // PUT /api/comissoes-evento/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarComissaoEventoRequest body, CancellationToken ct)
    {
        var ce = await _db.Set<ComissaoEvento>().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (ce is null) return NotFound();

        ce.Observacoes = body.Observacoes;

        if (body.CoordenadorUsuarioId.HasValue)
        {
            var uid = body.CoordenadorUsuarioId.Value;
            if (uid != Guid.Empty && !await _db.Set<Usuario>().AnyAsync(u => u.Id == uid, ct))
                return BadRequest("Usuário coordenador não encontrado.");
            ce.CoordenadorUsuarioId = uid == Guid.Empty ? null : uid;
        }

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    // DELETE /api/comissoes-evento/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id, CancellationToken ct)
    {
        var ce = await _db.Set<ComissaoEvento>().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (ce is null) return NotFound();

        _db.Remove(ce);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}

/* ===== DTOs ===== */


public record ComissaoDto(Guid Id, string Nome, string? Slug, bool Ativa);
public record UsuarioSlimDto(Guid Id, string Nome, string Email);

public record InscricaoTrabalhadorDto(
    Guid Id,
    Guid InscricaoId,
    Guid ComissaoEventoId,
    int Nivel, // se você serializa enum como string, troque para string
    InscricaoSlimDto Inscricao
);

public record InscricaoSlimDto(Guid Id, Guid ParticipanteId, string ParticipanteNome);

public record UsuarioRoleDto(Guid Id, Guid UsuarioId, int Role, Guid? EventoId, Guid? ComissaoEventoId, UsuarioSlimDto Usuario);

/* ===== Requests ===== */
public record CriarComissoesParaEventoRequest(
    [Required] Guid EventoId,
    [Required] IEnumerable<Guid> ComissaoIds,
    IEnumerable<CoordenadorVinculo>? Coordenadores
);

public record CoordenadorVinculo(Guid ComissaoId, Guid UsuarioId);

public record AtualizarCoordenadorRequest(Guid? UsuarioId);

public record AtualizarComissaoEventoRequest(string? Observacoes, Guid? CoordenadorUsuarioId);
