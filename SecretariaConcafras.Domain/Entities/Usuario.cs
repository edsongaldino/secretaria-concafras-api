using System.ComponentModel.DataAnnotations;

namespace SecretariaConcafras.Domain.Entities
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Nome { get; set; }

        [Required, MaxLength(150), EmailAddress]
        public string Email { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [Required, MaxLength(255)]
        public string Senha { get; set; }

        // Participante vinculado (quando aplicável)
        public Guid? ParticipanteId { get; set; }
        public Participante? Participante { get; set; }

        // Roles dinâmicas
        public ICollection<UsuarioRole> Roles { get; set; } = new List<UsuarioRole>();
    }
}
