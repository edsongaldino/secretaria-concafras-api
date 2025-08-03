namespace SecretariaConcafras.Application.DTOs.Pagamentos
{
    public class PagamentoResponseDto
    {
        public Guid Id { get; set; }
        public Guid InscricaoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public string MetodoPagamento { get; set; }
        public string Status { get; set; }
        public string? TransactionId { get; set; }
    }
}
