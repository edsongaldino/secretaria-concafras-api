using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class CheckinMapping : IEntityTypeConfiguration<Checkin>
    {
        public void Configure(EntityTypeBuilder<Checkin> builder)
        {
            builder.ToTable("checkins");
            builder.HasKey(x => x.Id).HasName("pk_checkins");


            builder.HasOne<Participante>()
            .WithMany()
            .HasForeignKey(x => x.ParticipanteId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_checkins__participantes__participante_id");


            builder.HasOne<Evento>()
            .WithMany()
            .HasForeignKey(x => x.EventoId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_checkins__eventos__evento_id");


            builder.HasOne<Curso>()
            .WithMany()
            .HasForeignKey(x => x.CursoId)
            .HasConstraintName("fk_checkins__cursos__curso_id");


            builder.HasIndex(x => x.ParticipanteId).HasDatabaseName("ix_checkins__participante_id");
            builder.HasIndex(x => x.EventoId).HasDatabaseName("ix_checkins__evento_id");
            builder.HasIndex(x => x.CursoId).HasDatabaseName("ix_checkins__curso_id");
        }
    }
}
