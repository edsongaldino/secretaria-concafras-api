using System.ComponentModel.DataAnnotations;

namespace SecretariaConcafras.Domain.Entities
{
    public class Instituicao
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(150)]
        public string Nome { get; set; }

        public Guid EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

        public ICollection<Participante> Participantes { get; set; } = new List<Participante>();
    }
}
