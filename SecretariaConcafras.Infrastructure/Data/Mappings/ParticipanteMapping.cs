using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ParticipanteMapping : IEntityTypeConfiguration<Participante>
{
    public void Configure(EntityTypeBuilder<Participante> builder)
    {
        builder.ToTable("participantes");
        builder.HasKey(p => p.Id).HasName("pk_participantes");

        builder.Property(x => x.Nome).HasMaxLength(150).IsRequired();
        builder.Property(x => x.CPF).HasMaxLength(11).IsRequired();

        builder.HasIndex(x => x.CPF).IsUnique();

        // FK opcional para Endereco
        builder.HasOne(x => x.Endereco)
         .WithMany()
         .HasForeignKey(x => x.EnderecoId)
         .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Instituicao)
         .WithMany() // ou .WithMany(i => i.Participantes) se tiver a coleção
         .HasForeignKey(p => p.InstituicaoId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}
