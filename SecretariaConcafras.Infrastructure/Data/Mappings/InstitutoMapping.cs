using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class InstitutoMapping : IEntityTypeConfiguration<Instituto>
    {
        public void Configure(EntityTypeBuilder<Instituto> builder)
        {
            builder.ToTable("institutos");
            builder.HasKey(x => x.Id).HasName("pk_institutos");
            builder.Property(x => x.Nome).IsRequired();
        }
    }
}
