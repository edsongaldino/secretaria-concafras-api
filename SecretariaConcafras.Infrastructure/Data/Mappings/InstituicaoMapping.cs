using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class InstituicaoMapping : IEntityTypeConfiguration<Instituicao>
    {
        public void Configure(EntityTypeBuilder<Instituicao> b)
        {
            b.ToTable("instituicoes");
            b.HasKey(x => x.Id).HasName("pk_instituicoes");
            b.Property(x => x.Nome).HasMaxLength(200).IsRequired();
            b.Property(x => x.NomeNormalizado).HasMaxLength(200).IsRequired();
            b.HasIndex(x => x.NomeNormalizado).IsUnique();
        }
    }
}
