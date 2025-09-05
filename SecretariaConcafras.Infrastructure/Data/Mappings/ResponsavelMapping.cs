using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class ResponsavelMapping : IEntityTypeConfiguration<Responsavel>
    {
        public void Configure(EntityTypeBuilder<Responsavel> builder)
        {
            builder.ToTable("responsaveis");
            builder.HasKey(x => x.Id).HasName("pk_responsaveis");
            builder.Property(x => x.Nome).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Parentesco).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Telefone).HasMaxLength(20).IsRequired();
        }
    }
}
