using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Domain.Entities
{
    public class Pagamento
    {
        public Guid Id { get; set; }
        public Guid EventoId { get; set; }
        public Guid ResponsavelFinanceiroId { get; set; }
        public decimal ValorTotal { get; set; }
        public MetodoPagamento Metodo { get; set; }
        public PagamentoStatus Status { get; set; } = PagamentoStatus.Pendente;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiraEm { get; set; }
        public string? ProviderReference { get; set; } // preference_id/payment_id
        public string? ProviderPayload { get; set; }

        public List<PagamentoItem> Itens { get; set; } = new();
    }
}
