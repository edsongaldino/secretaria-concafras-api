namespace SecretariaConcafras.Application.DTOs.Inscricoes
{
    public class InscricaoUpdateDto
    {
        public Guid Id { get; set; }
        public Guid EventoId { get; set; }
        public Guid ParticipanteId { get; set; }
        public Guid ResponsavelFinanceiroId { get; set; }
        public DateTime DataInscricao { get; set; }
        public decimal ValorInscricao { get; set; }

        // Campos de tela
        public string? ParticipanteNome { get; set; }
        public string? EventoTitulo { get; set; }

        // Seleções
        public bool EhTrabalhador { get; set; }
        public Guid? ComissaoEventoId { get; set; }
        public List<Guid> CursoIds { get; set; } = new();
    }

}
