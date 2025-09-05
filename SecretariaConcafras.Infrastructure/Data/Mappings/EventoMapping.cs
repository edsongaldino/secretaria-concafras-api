using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure.Mappings
{
    public class EventoMapping : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.ToTable("eventos");
            builder.HasKey(x => x.Id).HasName("pk_eventos");

            // Colunas (se você está usando snake_case)
            builder.Property(x => x.Titulo).IsRequired().HasColumnName("titulo");
            builder.Property(x => x.DataInicio).HasColumnName("data_inicio");
            builder.Property(x => x.DataFim).HasColumnName("data_fim");
            builder.Property(x => x.InicioInscricoes).HasColumnName("inicio_inscricoes");
            builder.Property(x => x.FimInscricoes).HasColumnName("fim_inscricoes");
            builder.Property(x => x.ValorInscricaoAdulto).HasColumnName("valor_inscricao_adulto"); // opcional: .HasColumnType("numeric(12,2)")
            builder.Property(x => x.ValorInscricaoCrianca).HasColumnName("valor_inscricao_crianca"); // opcional: .HasColumnType("numeric(12,2)")

            // FK EnderecoId -> endereco_id
            builder.Property(x => x.EnderecoId).HasColumnName("endereco_id");

            // RELAÇÃO CORRETA (usa navigation + FK explícita)
            builder
                .HasOne(e => e.Endereco)           // ? usa a navigation existente
                .WithMany()                         // Endereco não tem coleção de Eventos
                .HasForeignKey(e => e.EnderecoId)   // ? amarra à propriedade FK
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict)  // evitar cascata indesejada
                .HasConstraintName("fk_eventos__enderecos__endereco_id");

            builder.HasIndex(x => x.EnderecoId)
                   .HasDatabaseName("ix_eventos__endereco_id");
        }
    }
}
