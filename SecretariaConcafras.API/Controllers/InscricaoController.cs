using Microsoft.AspNetCore.Mvc;
using SecretariaConcafras.Application.DTOs.Inscricoes;
using SecretariaConcafras.Application.Interfaces.Services;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscricaoController : ControllerBase
    {
        private readonly IInscricaoService _service;

        public InscricaoController(IInscricaoService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var result = await _service.ObterPorIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> ObterPorEvento(Guid eventoId)
        {
            var result = await _service.ObterPorEventoAsync(eventoId);
            return Ok(result);
        }

        [HttpGet("participante/{participanteId}")]
        public async Task<IActionResult> ObterPorParticipante(Guid participanteId)
        {
            var result = await _service.ObterPorParticipanteAsync(participanteId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] InscricaoCreateDto dto)
        {
            var result = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancelar(Guid id)
        {
            var result = await _service.CancelarAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
