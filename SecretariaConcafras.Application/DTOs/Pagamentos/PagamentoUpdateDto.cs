namespace SecretariaConcafras.Application.DTOs.Pagamentos
{
    public class PagamentoUpdateDto
    {
        public string Status { get; set; } // Pendente, Pago, Cancelado
        public string? TransactionId { get; set; }
    }
}
