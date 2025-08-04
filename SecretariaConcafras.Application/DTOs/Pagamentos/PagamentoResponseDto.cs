using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Pagamentos
{
    public class PagamentoResponseDto
    {
        public Guid Id { get; set; }
        public Guid InscricaoId { get; set; }
        public decimal Valor { get; set; }

        // Calculado a partir do histórico (primeiro aprovado)
        public DateTime? DataPagamento { get; set; }

        public string? MetodoPagamento { get; set; }
        public StatusPagamento Status { get; set; } // mantém enum
        public string? CodigoTransacao { get; set; } // renomeado corretamente
    }
}
