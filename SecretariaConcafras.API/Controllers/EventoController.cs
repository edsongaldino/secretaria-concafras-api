using Microsoft.AspNetCore.Mvc;
using SecretariaConcafras.Application.DTOs.Eventos;
using SecretariaConcafras.Application.Interfaces.Services;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/evento")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _service;

        public EventoController(IEventoService service)
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

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] EventoCreateDto dto)
        {
            var result = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] EventoUpdateDto dto)
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
