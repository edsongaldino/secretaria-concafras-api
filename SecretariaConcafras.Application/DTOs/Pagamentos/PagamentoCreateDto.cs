using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Pagamentos
{
    public class PagamentoCreateDto
    {
        public Guid InscricaoId { get; set; }
        public decimal Valor { get; set; }
        public string MetodoPagamento { get; set; } // Ex: "CartaoCredito", "Pix"
    }

    public class PagamentoCreateResultDto
    {
        public Guid PagamentoId { get; set; }
        public Guid EventoId { get; set; }
        public Guid ResponsavelFinanceiroId { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = "Pendente";

        // usado no Checkout Pro (Mercado Pago redireciona)
        public string? CheckoutUrl { get; set; }

        // se um dia usar PIX direto, pode aproveitar os campos abaixo:
        public string? PixCopyPaste { get; set; }
        public string? QrCodeBase64 { get; set; }
        public DateTime? ExpiraEm { get; set; }
        public string? Mensagem { get; set; }
    }

    public class CriarPagamentoGrupoDto
    {
        public Guid EventoId { get; set; }
        public Guid ResponsavelFinanceiroId { get; set; }

        // hoje só usa Checkout, mas deixamos aberto
        public MetodoPagamento Metodo { get; set; } = MetodoPagamento.Checkout;
    }
}
