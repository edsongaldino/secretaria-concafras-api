using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Checkins
{
    public class CheckinCreateDto
    {
        public Guid ParticipanteId { get; set; }
        public Guid EventoId { get; set; }

        // Se for check-in para curso
        public Guid? CursoId { get; set; }

        public TipoCheckin Tipo { get; set; }
    }
}
