using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings
{
    public class CursoMapping : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.ToTable("Cursos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(c => c.Evento)
                .WithMany(e => e.Cursos)
                .HasForeignKey(c => c.EventoId);

            builder.HasOne(c => c.Instituto)
                .WithMany(i => i.Cursos)
                .HasForeignKey(c => c.InstitutoId);
        }
    }
}