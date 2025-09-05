using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class CursoMapping : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.ToTable("cursos");
            builder.HasKey(x => x.Id).HasName("pk_cursos");

            // Se NÃO usa UseSnakeCaseNamingConvention(), descomente:
            // builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.EventoId).HasColumnName("evento_id");
            builder.Property(x => x.InstitutoId).HasColumnName("instituto_id");

            // Curso -> Evento (N:1)
            builder.HasOne(c => c.Evento)
                   .WithMany(e => e.Cursos)
                   .HasForeignKey(c => c.EventoId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_cursos__eventos__evento_id");

            // Curso -> Instituto (N:1)  *** AQUI ***
            builder.HasOne(c => c.Instituto)            // use a navigation
                   .WithMany(i => i.Cursos)             // coleção em Instituto
                   .HasForeignKey(c => c.InstitutoId)   // amarra a FK explícita
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_cursos__institutos__instituto_id");
        }
    }
}
