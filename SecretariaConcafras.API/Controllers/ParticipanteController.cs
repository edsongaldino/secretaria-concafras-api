using Microsoft.AspNetCore.Mvc;
using SecretariaConcafras.Application.DTOs;
using SecretariaConcafras.Application.DTOs.Participantes;
using SecretariaConcafras.Application.Interfaces;
using SecretariaConcafras.Application.Interfaces.Services;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipanteController : ControllerBase
    {
        private readonly IParticipanteService _participanteService;

        public ParticipanteController(IParticipanteService participanteService)
        {
            _participanteService = participanteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _participanteService.ObterTodosAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var participante = await _participanteService.ObterPorIdAsync(id);
            if (participante == null) return NotFound();
            return Ok(participante);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParticipanteCreateDto dto)
        {
            var participante = await _participanteService.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = participante.Id }, participante);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ParticipanteUpdateDto dto)
        {
            var result = await _participanteService.AtualizarAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _participanteService.RemoverAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
