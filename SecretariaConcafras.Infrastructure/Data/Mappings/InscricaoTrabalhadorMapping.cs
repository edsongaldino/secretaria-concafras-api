using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class InscricaoTrabalhadorMapping : IEntityTypeConfiguration<InscricaoTrabalhador>
    {
        public void Configure(EntityTypeBuilder<InscricaoTrabalhador> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
