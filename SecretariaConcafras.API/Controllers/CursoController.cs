using Microsoft.AspNetCore.Mvc;
using SecretariaConcafras.Application.DTOs.Cursos;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/curso")]
    public class CursoController : ControllerBase
    {
        private readonly ICursoService _service;

        public CursoController(ICursoService service)
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
        public async Task<IActionResult> ObterPorEventoId(
        Guid eventoId,
        [FromQuery] string? publico,
        [FromQuery] string? bloco,
        [FromQuery] bool? neofito)
        {
            // parse opcional de "publico"
            PublicoCurso? publicoEnum = null;
            if (!string.IsNullOrWhiteSpace(publico))
            {
                if (int.TryParse(publico, out var pNum) && Enum.IsDefined(typeof(PublicoCurso), pNum))
                    publicoEnum = (PublicoCurso)pNum;
                else if (Enum.TryParse<PublicoCurso>(publico, ignoreCase: true, out var pNamed))
                    publicoEnum = pNamed;
                else
                    return BadRequest("Parâmetro 'publico' inválido. Valores aceitos: Crianca|Jovem|Adulto ou 1|2|3.");
            }

            // parse opcional de "bloco"
            BlocoCurso? blocoEnum = null;
            if (!string.IsNullOrWhiteSpace(bloco))
            {
                if (int.TryParse(bloco, out var bNum) && Enum.IsDefined(typeof(BlocoCurso), bNum))
                    blocoEnum = (BlocoCurso)bNum;
                else if (Enum.TryParse<BlocoCurso>(bloco, ignoreCase: true, out var bNamed))
                    blocoEnum = bNamed;
                else
                    return BadRequest("Parâmetro 'bloco' inválido. Valores aceitos: TemaAtual|TemaEspecifico ou 1|2.");
            }

            // chama o serviço passando filtros opcionais
            var result = await _service.ObterPorEventoAsync(eventoId, publicoEnum, blocoEnum, neofito);

            if (result == null || !result.Any())
                return NotFound("Nenhum curso encontrado com esses parâmetros.");

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CursoCreateDto dto)
        {
            var result = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] CursoUpdateDto dto)
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
