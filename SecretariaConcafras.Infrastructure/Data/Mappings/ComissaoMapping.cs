using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ComissaoMapping : IEntityTypeConfiguration<Comissao>
{
    public void Configure(EntityTypeBuilder<Comissao> b)
    {
        b.ToTable("comissoes");
        b.HasKey(x => x.Id).HasName("pk_comissoes");

        b.Property(x => x.Nome).HasColumnName("nome").IsRequired().HasMaxLength(120);
        b.Property(x => x.Slug).HasColumnName("slug").HasMaxLength(120);
        b.Property(x => x.Ativa).HasColumnName("ativa").HasDefaultValue(true);
    }
}
