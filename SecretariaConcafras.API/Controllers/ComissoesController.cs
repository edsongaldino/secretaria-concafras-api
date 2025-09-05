using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Domain.Entities; // Comissao
using SecretariaConcafras.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace SecretariaConcafras.API.Controllers;

[ApiController]
[Route("api/comissoes")]
[Produces(MediaTypeNames.Application.Json)]
public class ComissoesController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public ComissoesController(ApplicationDbContext db) => _db = db;

    // GET /api/comissoes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComissaoDto>>> Listar(CancellationToken ct)
    {
        var data = await _db.Set<Comissao>()
            .AsNoTracking()
            .OrderBy(c => c.Nome)
            .Select(c => new ComissaoDto(c.Id, c.Nome, c.Slug, c.Ativa))
            .ToListAsync(ct);

        return Ok(data);
    }

    // GET /api/comissoes/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ComissaoDto>> Obter(Guid id, CancellationToken ct)
    {
        var c = await _db.Set<Comissao>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (c is null) return NotFound();
        return Ok(new ComissaoDto(c.Id, c.Nome, c.Slug, c.Ativa));
    }

    // POST /api/comissoes
    [HttpPost]
    public async Task<ActionResult<Guid>> Criar([FromBody] CriarComissaoRequest body, CancellationToken ct)
    {
        var entity = new Comissao { Id = Guid.NewGuid(), Nome = body.Nome.Trim(), Slug = body.Slug, Ativa = body.Ativa };
        _db.Add(entity);
        await _db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Obter), new { id = entity.Id }, entity.Id);
    }

    // PUT /api/comissoes/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarComissaoRequest body, CancellationToken ct)
    {
        var entity = await _db.Set<Comissao>().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();

        entity.Nome = body.Nome.Trim();
        entity.Slug = body.Slug;
        entity.Ativa = body.Ativa;

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    // DELETE /api/comissoes/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id, CancellationToken ct)
    {
        var entity = await _db.Set<Comissao>().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();

        _db.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}

/* ===== DTOs ===== */
public record ComissaoDto(Guid Id, string Nome, string? Slug, bool Ativa);
public record CriarComissaoRequest([Required, MaxLength(120)] string Nome, string? Slug, bool Ativa = true);
public record AtualizarComissaoRequest([Required, MaxLength(120)] string Nome, string? Slug, bool Ativa = true);
