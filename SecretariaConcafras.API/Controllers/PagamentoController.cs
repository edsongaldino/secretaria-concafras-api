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
        public async Task<IActionResult> CriarParaGrupo([FromBody] CheckoutGrupoDto dto)
        {
            try
            {
                var res = await _service.CriarParaGrupoCheckoutAsync(dto.EventoId, dto.ResponsavelFinanceiroId, dto.ExcluirInscricaoIds);
                return Ok(res);
            }
            catch (ApplicationException aex) // erro vindo do gateway (Mercado Pago)
            {
                // opcional: parse do status embutido na mensagem; por padrão, 502
                return StatusCode(502, new { code = "MERCADOPAGO_ERROR", message = aex.Message });
            }
        }

        // status (para polling)
        [HttpGet("{id:guid}/status")]
        public async Task<ActionResult<object>> Status(Guid id)
        {
            var st = await _service.ObterStatusAsync(id);
            return Ok(new { status = st.ToString() });
        }

        [HttpGet("status/{pagamentoId:guid}")]
        public async Task<IActionResult> ObterStatus(Guid pagamentoId)
        {
            var status = await _service.ObterStatusAsync(pagamentoId);
            return Ok(new { pagamentoId, status = status.ToString() });
        }
    }
}
