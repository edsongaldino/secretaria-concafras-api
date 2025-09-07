using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;
using System.Reflection.Emit;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class PagamentoItemMapping : IEntityTypeConfiguration<PagamentoItem>
    {
        public void Configure(EntityTypeBuilder<PagamentoItem> b)
        {
            b.ToTable("pagamentos_itens");
            b.HasKey(x => x.Id);
            b.Property(x => x.PagamentoId).HasColumnName("pagamento_id");
            b.Property(x => x.InscricaoId).HasColumnName("inscricao_id");
            b.Property(x => x.Valor).HasColumnName("valor").HasColumnType("numeric(12,2)");
            b.HasIndex(x => new { x.PagamentoId, x.InscricaoId }).IsUnique();

            b.HasOne(pi => pi.Inscricao)
               .WithOne(i => i.PagamentoItem)
               .HasForeignKey<PagamentoItem>(pi => pi.InscricaoId)
               .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(pi => pi.InscricaoId).IsUnique();
        }
    }
}