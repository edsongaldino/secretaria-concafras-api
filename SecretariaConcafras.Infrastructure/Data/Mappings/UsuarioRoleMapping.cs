using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings;

public class UsuarioRoleMapping : IEntityTypeConfiguration<UsuarioRole>
{
    public void Configure(EntityTypeBuilder<UsuarioRole> b)
    {
        b.ToTable("usuario_roles");
        b.HasKey(x => x.Id).HasName("pk_usuario_roles");

        b.Property(x => x.UsuarioId).HasColumnName("usuario_id").IsRequired();
        b.Property(x => x.EventoId).HasColumnName("evento_id");
        b.Property(x => x.ComissaoEventoId).HasColumnName("comissao_evento_id");
        b.Property(x => x.Role).HasColumnName("role").IsRequired();

        b.HasOne(x => x.Usuario)
            .WithMany(u => u.Roles)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_usuario_roles_usuarios_usuario_id");

        b.HasOne(x => x.Evento)
            .WithMany(e => e.UsuarioRoles)
            .HasForeignKey(x => x.EventoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_usuario_roles_eventos_evento_id");

        b.HasOne(x => x.ComissaoEvento)
            .WithMany(ce => ce.UsuarioRoles)
            .HasForeignKey(x => x.ComissaoEventoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_usuario_roles_comissoes_evento_comissao_evento_id");
    }
}