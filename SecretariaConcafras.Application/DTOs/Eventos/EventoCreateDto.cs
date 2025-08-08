using SecretariaConcafras.Application.DTOs.Enderecos;

namespace SecretariaConcafras.Application.DTOs.Eventos
{
    public class EventoCreateDto
    {
        public string Titulo { get; set; } = default!;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime InscricaoInicio { get; set; }
        public DateTime InscricaoFim { get; set; }
        public decimal ValorInscricaoCrianca { get; set; }
        public decimal ValorInscricaoAdulto { get; set; }
        public string? BannerUrl { get; set; }

        // FLEX: aceite um OU outro
        public Guid? EnderecoId { get; set; }
        public EnderecoCreateDto? Endereco { get; set; }
    }
}
