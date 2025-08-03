using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings
{
    public class ParticipanteMapping : IEntityTypeConfiguration<Participante>
    {
        public void Configure(EntityTypeBuilder<Participante> builder)
        {
            builder.ToTable("Participantes");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.NomeCompleto)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.CPF)
                .IsRequired()
                .HasMaxLength(11);

            builder.HasOne(p => p.Instituicao)
                .WithMany(i => i.Participantes)
                .HasForeignKey(p => p.InstituicaoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Responsavel)
                .WithMany(r => r.Participantes)
                .HasForeignKey(p => p.ResponsavelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}