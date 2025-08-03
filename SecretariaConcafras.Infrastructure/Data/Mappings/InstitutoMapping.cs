using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings
{
    public class InstitutoMapping : IEntityTypeConfiguration<Instituto>
    {
        public void Configure(EntityTypeBuilder<Instituto> builder)
        {
            builder.ToTable("Institutos");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasMany(i => i.Cursos)
                .WithOne(c => c.Instituto)
                .HasForeignKey(c => c.InstitutoId);
        }
    }
}