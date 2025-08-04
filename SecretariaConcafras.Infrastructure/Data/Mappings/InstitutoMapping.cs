using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class InstitutoMapping : IEntityTypeConfiguration<Instituto>
    {
        public void Configure(EntityTypeBuilder<Instituto> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
