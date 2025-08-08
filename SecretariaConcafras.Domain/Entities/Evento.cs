namespace SecretariaConcafras.Domain.Entities
{
    public class Evento
    {
        public Guid Id { get; set; }
        public required string Titulo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime InicioInscricoes { get; set; }
        public DateTime FimInscricoes { get; set; }
        public decimal ValorInscricaoAdulto { get; set; }
        public decimal ValorInscricaoCrianca { get; set; }

        // Endereço
        public Guid? EnderecoId { get; set; }
        public Endereco? Endereco { get; set; }

        public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
        public ICollection<ComissaoTrabalho> Comissoes { get; set; } = new List<ComissaoTrabalho>();
        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();
    }
}
