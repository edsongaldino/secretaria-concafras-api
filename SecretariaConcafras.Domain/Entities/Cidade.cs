namespace SecretariaConcafras.Domain.Entities
{
    public class Cidade
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public Guid EstadoId { get; set; }
        public Estado Estado { get; set; }

        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
    }
}
