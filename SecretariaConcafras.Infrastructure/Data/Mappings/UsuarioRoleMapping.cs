using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class UsuarioRoleMapping : IEntityTypeConfiguration<UsuarioRole>
    {
        public void Configure(EntityTypeBuilder<UsuarioRole> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
