namespace SecretariaConcafras.Application.DTOs.Eventos
{
    public class EventoCreateDto
    {
        public string Titulo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime InscricaoInicio { get; set; }
        public DateTime InscricaoFim { get; set; }
        public decimal ValorInscricaoAdulto { get; set; }
        public decimal ValorInscricaoCrianca { get; set; }

        public Guid EnderecoId { get; set; }
    }
}
