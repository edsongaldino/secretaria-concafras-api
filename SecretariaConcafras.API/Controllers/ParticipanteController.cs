// API/Controllers/ParticipanteController.cs  (trecho)
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
}
