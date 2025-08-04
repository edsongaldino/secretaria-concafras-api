using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Checkins
{
    public class CheckinResponseDto
    {
        public Guid Id { get; set; }

        public Guid ParticipanteId { get; set; }
        public string ParticipanteNome { get; set; }

        public Guid EventoId { get; set; }
        public string EventoTitulo { get; set; }

        public Guid? CursoId { get; set; }
        public string? CursoTitulo { get; set; }

        public TipoCheckin Tipo { get; set; }

        public DateTime DataHora { get; set; }
    }
}
