using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ComissaoEventoMapping : IEntityTypeConfiguration<ComissaoEvento>
{
    public void Configure(EntityTypeBuilder<ComissaoEvento> b)
    {
        b.ToTable("comissoes_evento");
        b.HasKey(x => x.Id).HasName("pk_comissoes_evento");

        b.Property(x => x.EventoId).HasColumnName("evento_id").IsRequired();
        b.Property(x => x.ComissaoId).HasColumnName("comissao_id").IsRequired();
        b.Property(x => x.CoordenadorUsuarioId).HasColumnName("coordenador_usuario_id");
        b.Property(x => x.Observacoes).HasColumnName("observacoes");

        b.HasOne(x => x.Evento)
            .WithMany(e => e.ComissoesEvento)
            .HasForeignKey(x => x.EventoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_comissoes_evento_eventos_evento_id");

        b.HasOne(x => x.Comissao)
            .WithMany(c => c.ComissoesEvento)
            .HasForeignKey(x => x.ComissaoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_comissoes_evento_comissoes_comissao_id");

        b.HasOne(x => x.CoordenadorUsuario)
            .WithMany() // ou .WithMany(u => u.ComissoesCoordenadas) se quiser coleção reversa
            .HasForeignKey(x => x.CoordenadorUsuarioId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_comissoes_evento_usuarios_coordenador_usuario_id");

        // cada comissão aparece no máximo uma vez por evento
        b.HasIndex(x => new { x.EventoId, x.ComissaoId })
            .IsUnique()
            .HasDatabaseName("ux_comissoes_evento_evento_comissao");
    }
}