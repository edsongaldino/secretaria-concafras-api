using Microsoft.AspNetCore.Mvc;
using SecretariaConcafras.Application.DTOs.Pagamentos;
using SecretariaConcafras.Application.Interfaces;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Application.Services;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/pagamentos")]
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoService _service;

        public PagamentoController(IPagamentoService service)
        {
            _service = service;
        }

        // cria pagamento (grupo)
        [HttpPost("checkout/grupo")]
        public async Task<IActionResult> CriarParaGrupo(Guid eventoId, Guid responsavelId)
        {
            var result = await _service.CriarParaGrupoCheckoutAsync(eventoId, responsavelId);
            return Ok(result);
        }

        // status (para polling)
        [HttpGet("{id:guid}/status")]
        public async Task<ActionResult<object>> Status(Guid id)
        {
            var st = await _service.ObterStatusAsync(id);
            return Ok(new { status = st.ToString() });
        }
    }
}
