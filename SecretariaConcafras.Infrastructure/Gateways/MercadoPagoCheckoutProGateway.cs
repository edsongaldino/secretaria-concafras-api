using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Error;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IGatewayPagamento
{
    Task<GatewayCreateResult> CriarCheckoutGrupoAsync(
        Guid pagamentoId,
        decimal valor,
        string descricao,
        string notificationUrl,
        string successUrl,
        string failureUrl,
        string pendingUrl);

    Task<GatewayPaymentStatus> ObterStatusPorPaymentIdAsync(long mpPaymentId);
}

public record GatewayCreateResult(string ProviderRef, string CheckoutUrl);

public enum GatewayPaymentStatus
{
    Pendente,
    Aguardando,
    Pago,
    Cancelado,
    Expirado,
    Falhou
}

public class MercadoPagoCheckoutProGateway : IGatewayPagamento
{
    private readonly ILogger<MercadoPagoCheckoutProGateway> _logger;

    public MercadoPagoCheckoutProGateway(ILogger<MercadoPagoCheckoutProGateway> logger)
    {
        _logger = logger;
    }

    public async Task<GatewayCreateResult> CriarCheckoutGrupoAsync(
        Guid pagamentoId,
        decimal valor,
        string desc,
        string notificationUrl,
        string successUrl,
        string failureUrl,
        string pendingUrl)
    {
        // 1) Validações rápidas
        if (valor <= 0m)
            throw new ArgumentOutOfRangeException(nameof(valor), "Valor deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(notificationUrl) || !notificationUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("notificationUrl inválida ou sem HTTPS.", nameof(notificationUrl));

        if (string.IsNullOrWhiteSpace(successUrl) || !successUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("successUrl inválida ou sem HTTPS.", nameof(successUrl));

        if (string.IsNullOrWhiteSpace(failureUrl)) failureUrl = successUrl;
        if (string.IsNullOrWhiteSpace(pendingUrl)) pendingUrl = successUrl;

        if (string.IsNullOrWhiteSpace(desc)) desc = $"Pagamento {pagamentoId}";
        if (desc.Length > 256) desc = desc[..256];

        // 2) AccessToken deve estar setado no startup
        if (string.IsNullOrWhiteSpace(MercadoPagoConfig.AccessToken))
            throw new InvalidOperationException("MercadoPago AccessToken não configurado.");

        // 3) Monta a preferência
        var client = new PreferenceClient();
        var req = new PreferenceRequest
        {
            Items = new List<PreferenceItemRequest>
            {
                new PreferenceItemRequest
                {
                    Title = desc,
                    Quantity = 1,
                    UnitPrice = valor,
                    CurrencyId = "BRL"
                }
            },
            ExternalReference = pagamentoId.ToString(),
            NotificationUrl = notificationUrl,
            BackUrls = new PreferenceBackUrlsRequest
            {
                Success = successUrl,
                Failure = failureUrl,
                Pending = pendingUrl
            },
            AutoReturn = "approved"
        };

        try
        {
            // 4) Cria a preferência
            Preference pref = await client.CreateAsync(req);

            if (string.IsNullOrWhiteSpace(pref.Id))
                throw new InvalidOperationException("Mercado Pago não retornou preference_id.");

            if (string.IsNullOrWhiteSpace(pref.InitPoint))
                throw new InvalidOperationException("Mercado Pago não retornou a URL de checkout (init_point).");

            // Produção usa InitPoint (sandbox usa SandboxInitPoint)
            return new GatewayCreateResult(pref.Id!, pref.InitPoint!);
        }
        catch (MercadoPagoApiException ex)
        {
            // Log detalhado
            _logger.LogError(ex,
                "Erro MercadoPago ao criar preferência. Status={Status} Error={Error} Cause={Cause} Message={Message}",
                ex.StatusCode, ex.ApiError?.Error, ex.ApiError?.Cause, ex.Message);

            var status = ex.StatusCode == 0 ? 502 : (int)ex.StatusCode;
            // Sem ExternalGatewayException: use ApplicationException com detalhe de status
            throw new ApplicationException($"MERCADOPAGO_ERROR ({status}): {ex.ApiError?.Message ?? ex.Message}", ex);
        }
    }

    public async Task<GatewayPaymentStatus> ObterStatusPorPaymentIdAsync(long mpPaymentId)
    {
        var payClient = new PaymentClient();
        Payment pay = await payClient.GetAsync(mpPaymentId);

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
