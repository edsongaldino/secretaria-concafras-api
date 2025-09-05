using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;
using System.Reflection.Emit;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class PagamentoHistoricoMapping : IEntityTypeConfiguration<PagamentoHistorico>
    {
        public void Configure(EntityTypeBuilder<PagamentoHistorico> builder)
        {
			builder.ToTable("pagamentos_historicos");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.PagamentoId).HasColumnName("pagamento_id");

			builder.HasOne(x => x.Pagamento)
			 .WithMany(p => p.Historicos)
			 .HasForeignKey(x => x.PagamentoId)
			 .HasConstraintName("fk_pagamentos_historicos_pagamentos_pagamento_id");
		}
    }
}