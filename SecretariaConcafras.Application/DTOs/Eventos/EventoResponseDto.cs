using SecretariaConcafras.Application.DTOs.Cursos;

namespace SecretariaConcafras.Application.DTOs.Eventos
{
    public class EventoResponseDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime InscricaoInicio { get; set; }
        public DateTime InscricaoFim { get; set; }
        public decimal ValorInscricaoAdulto { get; set; }
        public decimal ValorInscricaoCrianca { get; set; }

        public string EnderecoCompleto { get; set; }

        public ICollection<CursoResponseDto> Cursos { get; set; } = new List<CursoResponseDto>();
    }
}
