using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings
{
    public class EquipeOrganizacaoMapping : IEntityTypeConfiguration<EquipeOrganizacao>
    {
        public void Configure(EntityTypeBuilder<EquipeOrganizacao> builder)
        {
            builder.ToTable("EquipesOrganizacao");

            builder.HasKey(eq => eq.Id);

            builder.HasOne(eq => eq.Evento)
                .WithMany(e => e.EquipeOrganizacao)
                .HasForeignKey(eq => eq.EventoId);
        }
    }
}