using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings
{
    public class EventoMapping : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.ToTable("Eventos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Endereco)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasMany(e => e.Cursos)
                .WithOne(c => c.Evento)
                .HasForeignKey(c => c.EventoId);

            builder.HasMany(e => e.EventoInstitutos)
                .WithOne(ei => ei.Evento)
                .HasForeignKey(ei => ei.EventoId);

            builder.HasMany(e => e.EquipeOrganizacao)
                .WithOne(eq => eq.Evento)
                .HasForeignKey(eq => eq.EventoId);
        }
    }
}