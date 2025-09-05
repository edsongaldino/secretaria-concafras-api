using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

public class InstitutoMapping : IEntityTypeConfiguration<Instituto>
{
    public void Configure(EntityTypeBuilder<Instituto> builder)
    {
        builder.ToTable("institutos");
        builder.HasKey(x => x.Id).HasName("pk_institutos");

        // Se NÃO usa UseSnakeCaseNamingConvention():
        // builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.Nome).IsRequired();
    }
}
