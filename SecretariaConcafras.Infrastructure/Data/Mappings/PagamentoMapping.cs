using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> b)
        {
            b.ToTable("pagamentos");
            b.HasKey(x => x.Id);
            b.Property(x => x.EventoId).HasColumnName("evento_id");
            b.Property(x => x.ResponsavelFinanceiroId).HasColumnName("responsavel_financeiro_id");
            b.Property(x => x.ValorTotal).HasColumnName("valor_total").HasColumnType("numeric(12,2)");
            b.Property(x => x.Metodo).HasColumnName("metodo").HasConversion<short>();
            b.Property(x => x.Status).HasColumnName("status").HasConversion<short>();
            b.Property(x => x.ExpiraEm).HasColumnName("expira_em");
            b.Property(x => x.ProviderReference).HasColumnName("provider_reference");
            b.Property(x => x.ProviderPayload).HasColumnName("provider_payload");
            b.HasMany(x => x.Itens).WithOne(i => i.Pagamento).HasForeignKey(i => i.PagamentoId);
            b.HasIndex(x => new { x.EventoId, x.ResponsavelFinanceiroId, x.Status })
             .HasDatabaseName("ix_pag_evento_resp_status");
        }
    }
}
