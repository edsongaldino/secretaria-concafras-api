using Microsoft.AspNetCore.Mvc;
using SecretariaConcafras.Application.DTOs.Pagamentos;
using SecretariaConcafras.Application.Interfaces;
using SecretariaConcafras.Application.Interfaces.Services;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoService _service;

        public PagamentoController(IPagamentoService service)
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
        public async Task<IActionResult> Criar([FromBody] PagamentoCreateDto dto)
        {
            var result = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] PagamentoUpdateDto dto)
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
