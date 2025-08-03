using SecretariaConcafras.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SecretariaConcafras.Domain.Entities
{
    public class UsuarioComissao
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [Required]
        public Guid ComissaoId { get; set; }
        public ComissaoTrabalho Comissao { get; set; }

        [Required]
        public PerfilComissao Perfil { get; set; }
    }
}
