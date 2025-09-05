using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.ToTable("pagamentos");
            builder.HasKey(x => x.Id).HasName("pk_pagamentos");


            builder.HasOne<Inscricao>()
            .WithMany()
            .HasForeignKey(x => x.InscricaoId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_pagamentos__inscricoes__inscricao_id");


            builder.HasIndex(x => x.InscricaoId).HasDatabaseName("ix_pagamentos__inscricao_id");
        }
    }
}
