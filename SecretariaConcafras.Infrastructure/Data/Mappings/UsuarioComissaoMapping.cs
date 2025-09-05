using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class UsuarioComissaoMapping : IEntityTypeConfiguration<UsuarioComissao>
    {
        public void Configure(EntityTypeBuilder<UsuarioComissao> builder)
        {
            builder.ToTable("usuario_comissoes");
            builder.HasKey(x => x.Id).HasName("pk_usuario_comissoes");


            builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_usuario_comissoes__usuarios__usuario_id");


            builder.HasOne<ComissaoEvento>()
            .WithMany()
            .HasForeignKey(x => x.ComissaoId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_usuario_comissoes__comissoes_trabalho__comissao_id");


            builder.HasIndex(x => x.UsuarioId).HasDatabaseName("ix_usuario_comissoes__usuario_id");
            builder.HasIndex(x => x.ComissaoId).HasDatabaseName("ix_usuario_comissoes__comissao_id");
        }
    }
}
