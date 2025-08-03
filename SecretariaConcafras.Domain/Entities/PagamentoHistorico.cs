using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Domain.Entities
{
    public class PagamentoHistorico
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid PagamentoId { get; set; }
        public Pagamento Pagamento { get; set; }

        public StatusPagamento Status { get; set; }
        public DateTime Data { get; set; } = DateTime.UtcNow;
        public string? Mensagem { get; set; } // logs de retorno do PagSeguro
    }
}
