namespace SecretariaConcafras.Domain.Entities
{
    public class Estado
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }

        public ICollection<Cidade> Cidades { get; set; } = new List<Cidade>();
    }
}
