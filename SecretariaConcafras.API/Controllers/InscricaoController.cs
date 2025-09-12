using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Inscricoes;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/inscricao")]
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

        [HttpPost("criar")]
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

        [HttpGet("lista-inscricoes/{eventoId}/{participanteId}")]
        public async Task<IActionResult> ListaInscricoesPorParticipante(Guid participanteId, Guid eventoId)
        {
            var result = await _service.ListaInscricoesAsync(eventoId,participanteId);
            return Ok(result);
        }

        [HttpGet("{id:guid}/full")]
        public async Task<InscricaoUpdateDto> GetFull(Guid id, [FromServices] ApplicationDbContext db)
        {
            var insc = await db.Inscricoes
            .Where(i => i.Id == id)
            .Select(i => new InscricaoUpdateDto
            {
                Id = i.Id,
                EventoId = i.EventoId,
                ParticipanteId = i.ParticipanteId,
                ResponsavelFinanceiroId = i.ResponsavelFinanceiroId,
                DataInscricao = i.DataInscricao,
                ValorInscricao = i.ValorInscricao,            // garanta não-nulo no banco
                ParticipanteNome = i.Participante.Nome,
                EventoTitulo = i.Evento.Titulo,
                EhTrabalhador = i.InscricaoTrabalhador != null,
                ComissaoEventoId = i.InscricaoTrabalhador != null
                    ? (Guid?)i.InscricaoTrabalhador.ComissaoEventoId
                    : null,
                CursoIds = i.Cursos.Select(c => c.CursoId).ToList()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

            return insc ?? new InscricaoUpdateDto();
        }
    }
}
