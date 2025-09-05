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

			builder.Property(x => x.InscricaoId).HasColumnName("inscricao_id");
			builder.Property(x => x.Valor).HasColumnName("valor");
			builder.Property(x => x.DataCriacao).HasColumnName("data_criacao");
			builder.Property(x => x.Status).HasColumnName("status");
			builder.Property(x => x.CodigoTransacao).HasColumnName("codigo_transacao");
			builder.Property(x => x.MetodoPagamento).HasColumnName("metodo_pagamento");
			builder.Property(x => x.QrCodePix).HasColumnName("qrcode_pix");

			builder.HasOne(x => x.Inscricao)
			 .WithMany(i => i.Pagamentos)
			 .HasForeignKey(x => x.InscricaoId)
			 .HasConstraintName("fk_pagamentos_inscricoes_inscricao_id")
			 .OnDelete(DeleteBehavior.Cascade); // opcional
		}
    }
}
