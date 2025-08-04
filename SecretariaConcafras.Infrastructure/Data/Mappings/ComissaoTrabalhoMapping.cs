using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class ComissaoTrabalhoMapping : IEntityTypeConfiguration<ComissaoTrabalho>
    {
        public void Configure(EntityTypeBuilder<ComissaoTrabalho> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
