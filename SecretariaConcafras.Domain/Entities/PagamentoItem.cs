using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaConcafras.Domain.Entities
{
    public class PagamentoItem
    {
        public Guid Id { get; set; }
        public Guid PagamentoId { get; set; }
        public Guid InscricaoId { get; set; }
        public decimal Valor { get; set; }
        public Pagamento Pagamento { get; set; } = default!;
        public Inscricao Inscricao { get; set; } = default!;
    }
}
