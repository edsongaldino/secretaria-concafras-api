using System.ComponentModel.DataAnnotations;

namespace SecretariaConcafras.Domain.Entities
{
    public class Responsavel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(150)]
        public string Nome { get; set; }

        [Required, MaxLength(50)]
        public string Parentesco { get; set; }

        [MaxLength(20)]
        public string Telefone { get; set; }
    }
}
