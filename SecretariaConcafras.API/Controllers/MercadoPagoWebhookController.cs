using Microsoft.AspNetCore.Mvc;
using SecretariaConcafras.Domain.Enums;
using SecretariaConcafras.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SecretariaConcafras.API.Controllers;

[ApiController]
public class MercadoPagoWebhookController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IGatewayPagamento _gateway;
    public MercadoPagoWebhookController(ApplicationDbContext db, IGatewayPagamento gateway)
    { _db = db; _gateway = gateway; }

    [HttpPost("api/pagamentos/mp/webhook")]
    public async Task<IActionResult> Webhook()
    {
        var type = Request.Query["type"].ToString();
        var idStr = Request.Query["data.id"].ToString();
        if (type != "payment" || string.IsNullOrEmpty(idStr)) return Ok();

        // 1) consulta o pagamento no MP
        var pay = await new MercadoPago.Client.Payment.PaymentClient().GetAsync(long.Parse(idStr));

        // 2) pega seu pagamentoId
        if (!Guid.TryParse(pay.ExternalReference, out var pagamentoId))
            return Ok(); // não é seu, ignore

        // 3) atualiza status local
        var p = await _db.Pagamentos
            .Include(x => x.Itens)
            .FirstOrDefaultAsync(x => x.Id == pagamentoId);

        if (p == null) return Ok();

        p.Status = pay.Status switch
        {
            "approved" => PagamentoStatus.Pago,
            "pending" or "in_process" => PagamentoStatus.Aguardando,
            "rejected" => PagamentoStatus.Falhou,
            "cancelled" => PagamentoStatus.Cancelado,
            "expired" => PagamentoStatus.Expirado,
            _ => p.Status
        };

        if (p.Status == PagamentoStatus.Pago)
        {
            var insIds = p.Itens.Select(i => i.InscricaoId).ToList();
            var inscricoes = await _db.Inscricoes.Where(i => insIds.Contains(i.Id)).ToListAsync();
            foreach (var i in inscricoes) i.PagamentoConfirmado = true;
        }

        await _db.SaveChangesAsync();
        return Ok();
    }
}
