using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

public class InscricaoTrabalhadorMapping : IEntityTypeConfiguration<InscricaoTrabalhador>
{
    public void Configure(EntityTypeBuilder<InscricaoTrabalhador> builder)
    {
        builder.ToTable("inscricoes_trabalhador");
        builder.HasKey(x => x.InscricaoId);
        builder.Property(x => x.InscricaoId).HasColumnName("inscricao_id");
        builder.Property(x => x.ComissaoEventoId).HasColumnName("comissao_evento_id");
        builder.Property(x => x.Nivel).HasColumnName("nivel");

        builder.HasOne(x => x.Inscricao)
         .WithOne(i => i.InscricaoTrabalhador)
         .HasForeignKey<InscricaoTrabalhador>(x => x.InscricaoId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}