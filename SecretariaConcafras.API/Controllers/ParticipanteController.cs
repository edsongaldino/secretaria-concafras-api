// API/Controllers/ParticipanteController.cs  (trecho)
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretariaConcafras.Application.DTOs.Participantes;
using SecretariaConcafras.Application.Interfaces.Services;

[ApiController]
[Route("api/participantes")]
public class ParticipanteController : ControllerBase
{
    private readonly IParticipanteService _svc;
    public ParticipanteController(IParticipanteService svc) => _svc = svc;

    [HttpPost("criar-ou-obter-por-cpf")]
    public async Task<ActionResult<ParticipanteResultadoDto>> CriarOuObterPorCpf(
        [FromBody] ParticipanteCreateDto dto, CancellationToken ct)
    {
        var result = await _svc.UpsertPorCpfAsync(dto, ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ParticipanteResponseDto>> ObterPorId(Guid id)
    {
        var dto = await _svc.ObterPorIdAsync(id);
        if (dto is null) return NotFound();
        return Ok(dto);
    }
}
