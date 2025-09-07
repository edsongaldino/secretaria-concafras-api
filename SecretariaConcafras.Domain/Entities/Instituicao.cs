using System.ComponentModel.DataAnnotations;

namespace SecretariaConcafras.Domain.Entities
{
    public class Instituicao
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = default!;
        public string NomeNormalizado { get; set; } = default!;
    }
}
