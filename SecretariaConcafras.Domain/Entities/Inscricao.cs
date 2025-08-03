namespace SecretariaConcafras.Domain.Entities
{
    public class Inscricao
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid EventoId { get; set; }
        public Evento Evento { get; set; }

        public Guid ParticipanteId { get; set; }
        public Participante Participante { get; set; }

        public DateTime DataInscricao { get; set; } = DateTime.UtcNow;
        public bool PagamentoConfirmado { get; set; } = false;

        // Relacionamentos auxiliares
        public InscricaoCurso? InscricaoCurso { get; set; }
        public InscricaoTrabalhador? InscricaoTrabalhador { get; set; }

        // Pagamentos realizados
        public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
    }
}
