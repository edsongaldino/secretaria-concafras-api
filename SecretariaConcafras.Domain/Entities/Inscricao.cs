namespace SecretariaConcafras.Domain.Entities
{
    public class Inscricao
    {
        public Guid Id { get; set; }
        public Guid EventoId { get; set; }
        public Guid ParticipanteId { get; set; }
        public Guid ResponsavelFinanceiroId { get; set; }
        public DateTime DataInscricao { get; set; }
        public bool PagamentoConfirmado { get; set; }
		public decimal ValorInscricao { get; set; }

		// Navegações
		public Participante Participante { get; set; } = default!;
        public Evento Evento { get; set; } = default!;
        public ICollection<InscricaoCurso> Cursos { get; set; } = new List<InscricaoCurso>();
        public InscricaoTrabalhador? InscricaoTrabalhador { get; set; }
        public PagamentoItem? PagamentoItem { get; set; }
    }
}
