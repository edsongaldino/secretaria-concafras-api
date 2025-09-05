using Microsoft.AspNetCore.Mvc;
using SecretariaConcafras.Application.DTOs.Comissoes;
using SecretariaConcafras.Application.Interfaces.Services;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/comissao-trabalho")]
    public class ComissaoTrabalhoController : ControllerBase
    {
        private readonly IComissaoService _service;

        public ComissaoTrabalhoController(IComissaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
            => Ok(await _service.ObterTodosAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var result = await _service.ObterPorIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpGet("obter-por-evento/{eventoId:guid}")]
        public async Task<IActionResult> ObterPorEventoId(Guid eventoId)
        {
            var result = await _service.ObterPorEventoAsync(eventoId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ComissaoTrabalhoCreateDto dto)
        {
            var result = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ComissaoTrabalhoUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var result = await _service.AtualizarAsync(dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var result = await _service.RemoverAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
