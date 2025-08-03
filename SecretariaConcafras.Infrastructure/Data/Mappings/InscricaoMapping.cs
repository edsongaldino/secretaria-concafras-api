using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings
{
    public class InscricaoMapping : IEntityTypeConfiguration<Inscricao>
    {
        public void Configure(EntityTypeBuilder<Inscricao> builder)
        {
            builder.ToTable("Inscricoes");

            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.Evento)
                .WithMany()
                .HasForeignKey(i => i.EventoId);

            builder.HasOne(i => i.Participante)
                .WithMany(p => p.Inscricoes)
                .HasForeignKey(i => i.ParticipanteId);

            builder.HasOne(i => i.Instituto)
                .WithMany()
                .HasForeignKey(i => i.InstitutoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}