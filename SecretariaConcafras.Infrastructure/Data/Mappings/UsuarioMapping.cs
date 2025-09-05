using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");
        builder.HasKey(x => x.Id).HasName("pk_usuarios");

        builder.Property(x => x.ParticipanteId).HasColumnName("participante_id");

        builder.HasMany(x => x.Roles)
            .WithOne(ur => ur.Usuario)
            .HasForeignKey(ur => ur.UsuarioId)
            .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_usuario_roles_usuarios_usuario_id");
    }
}
