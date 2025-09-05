using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class PagamentoHistoricoMapping : IEntityTypeConfiguration<PagamentoHistorico>
    {
        public void Configure(EntityTypeBuilder<PagamentoHistorico> builder)
        {
            builder.ToTable("pagamentos_historico");
            builder.HasKey(x => x.Id).HasName("pk_pagamentos_historico");


            builder.HasOne<Pagamento>()
            .WithMany()
            .HasForeignKey(x => x.PagamentoId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_pagamentos_historico__pagamentos__pagamento_id");


            builder.HasIndex(x => x.PagamentoId).HasDatabaseName("ix_pagamentos_historico__pagamento_id");
        }
    }
}
