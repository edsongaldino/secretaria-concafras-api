using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

public class InscricaoCursoMapping : IEntityTypeConfiguration<InscricaoCurso>
{
    public void Configure(EntityTypeBuilder<InscricaoCurso> builder)
    {
        builder.ToTable("inscricoes_curso");                  // <- use o nome exato da SUA tabela
        builder.HasKey(x => new { x.InscricaoId, x.CursoId }); // PK composta

        builder.Property(x => x.InscricaoId).HasColumnName("inscricao_id");
        builder.Property(x => x.CursoId).HasColumnName("curso_id");

        builder.HasOne(x => x.Inscricao)
         .WithMany(i => i.Cursos)
         .HasForeignKey(x => x.InscricaoId)
         .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Curso)
           .WithMany(c => c.Inscricoes)
           .HasForeignKey(x => x.CursoId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
