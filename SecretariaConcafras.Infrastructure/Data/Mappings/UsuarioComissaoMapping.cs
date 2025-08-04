using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class UsuarioComissaoMapping : IEntityTypeConfiguration<UsuarioComissao>
    {
        public void Configure(EntityTypeBuilder<UsuarioComissao> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
