using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        builder.ToTable("enderecos");
        builder.HasKey(x => x.Id).HasName("pk_enderecos");

        builder.Property(x => x.Id).HasColumnName("id");                 // <<< essencial
        builder.Property(x => x.Logradouro).HasColumnName("logradouro");
        builder.Property(x => x.Numero).HasColumnName("numero");
        builder.Property(x => x.Complemento).HasColumnName("complemento");
        builder.Property(x => x.Bairro).HasColumnName("bairro");
        builder.Property(x => x.Cidade).HasColumnName("cidade");
        builder.Property(x => x.Estado).HasColumnName("estado");
    }
}
