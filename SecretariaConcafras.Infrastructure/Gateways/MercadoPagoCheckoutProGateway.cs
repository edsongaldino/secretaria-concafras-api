using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public interface IGatewayPagamento
{
    Task<GatewayCreateResult> CriarCheckoutGrupoAsync(Guid pagamentoId, decimal valor, string descricao, string notificationUrl, string successUrl, string failureUrl, string pendingUrl);
    Task<GatewayPaymentStatus> ObterStatusPorPaymentIdAsync(long mpPaymentId);
}

public record GatewayCreateResult(string ProviderRef, string CheckoutUrl);
public enum GatewayPaymentStatus { Pendente, Aguardando, Pago, Cancelado, Expirado, Falhou }

public class MercadoPagoCheckoutProGateway : IGatewayPagamento
{
    public async Task<GatewayCreateResult> CriarCheckoutGrupoAsync(
        Guid pagamentoId, decimal valor, string desc,
        string notificationUrl, string successUrl, string failureUrl, string pendingUrl)
    {
        var client = new PreferenceClient();
        var req = new PreferenceRequest
        {
            Items = new List<PreferenceItemRequest> {
                new PreferenceItemRequest {
                    Title = desc,
                    Quantity = 1,
                    UnitPrice = valor,
                    CurrencyId = "BRL"
                }
            },
            ExternalReference = pagamentoId.ToString(),     // <- importantíssimo pra correlacionar no webhook
            NotificationUrl = notificationUrl,              // /api/pagamentos/mp/webhook
            BackUrls = new PreferenceBackUrlsRequest
            {
                Success = successUrl,
                Failure = failureUrl,
                Pending = pendingUrl
            },
            AutoReturn = "approved"
        };

        Preference pref = await client.CreateAsync(req);
        return new GatewayCreateResult(pref.Id!, pref.InitPoint);
    }

    public async Task<GatewayPaymentStatus> ObterStatusPorPaymentIdAsync(long mpPaymentId)
    {
        var pay = await new MercadoPago.Client.Payment.PaymentClient().GetAsync(mpPaymentId);
        return pay.Status switch
        {
            "approved" => GatewayPaymentStatus.Pago,
            "pending" or "in_process" => GatewayPaymentStatus.Aguardando,
            "rejected" => GatewayPaymentStatus.Falhou,
            "cancelled" => GatewayPaymentStatus.Cancelado,
            "expired" => GatewayPaymentStatus.Expirado,
            _ => GatewayPaymentStatus.Pendente
        };
    }
}
