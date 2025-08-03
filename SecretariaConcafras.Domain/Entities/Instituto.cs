namespace SecretariaConcafras.Domain.Entities
{
    public class Instituto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
    }
}
