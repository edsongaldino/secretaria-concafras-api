using System.ComponentModel.DataAnnotations;
using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Domain.Entities
{
    public class Checkin
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ParticipanteId { get; set; }
        public Participante Participante { get; set; }

        public Guid EventoId { get; set; }
        public Evento Evento { get; set; }

        public TipoCheckin Tipo { get; set; }

        public Guid? CursoId { get; set; }
        public Curso? Curso { get; set; }

        public DateTime DataHora { get; set; } = DateTime.UtcNow;
    }
}
