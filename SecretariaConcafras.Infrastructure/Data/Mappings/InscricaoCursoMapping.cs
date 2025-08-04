using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class InscricaoCursoMapping : IEntityTypeConfiguration<InscricaoCurso>
    {
        public void Configure(EntityTypeBuilder<InscricaoCurso> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
