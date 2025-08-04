using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class PagamentoHistoricoMapping : IEntityTypeConfiguration<PagamentoHistorico>
    {
        public void Configure(EntityTypeBuilder<PagamentoHistorico> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
