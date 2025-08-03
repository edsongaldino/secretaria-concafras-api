namespace SecretariaConcafras.Application.DTOs.Pagamentos
{
    public class PagamentoCreateDto
    {
        public Guid InscricaoId { get; set; }
        public decimal Valor { get; set; }
        public string MetodoPagamento { get; set; } // Ex: "CartaoCredito", "Pix"
    }
}
