using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings
{
    public class CheckinMapping : IEntityTypeConfiguration<Checkin>
    {
        public void Configure(EntityTypeBuilder<Checkin> builder)
        {
            builder.ToTable("Checkins");

            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Evento)
                .WithMany()
                .HasForeignKey(c => c.EventoId);

            builder.HasOne(c => c.Participante)
                .WithMany(p => p.Checkins)
                .HasForeignKey(c => c.ParticipanteId);

            builder.HasOne(c => c.Curso)
                .WithMany()
                .HasForeignKey(c => c.CursoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}