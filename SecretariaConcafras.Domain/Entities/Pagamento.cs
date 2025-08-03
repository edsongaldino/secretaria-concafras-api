using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Domain.Entities
{
    public class Pagamento
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid InscricaoId { get; set; }
        public Inscricao Inscricao { get; set; }

        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        // Status controlado pelo retorno do PagSeguro
        public StatusPagamento Status { get; set; } = StatusPagamento.Pendente;

        // Informações de transação no PagSeguro
        public string? CodigoTransacao { get; set; }
        public string? MetodoPagamento { get; set; } // PIX, Cartão, Boleto
        public string? QrCodePix { get; set; } // quando for PIX

        // Histórico de alterações
        public ICollection<PagamentoHistorico> Historicos { get; set; } = new List<PagamentoHistorico>();
    }
}
