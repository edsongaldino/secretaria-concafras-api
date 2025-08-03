using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Inscricoes
{
    public class InscricaoCreateDto
    {
        public Guid EventoId { get; set; }
        public Guid ParticipanteId { get; set; }

        public bool EhTrabalhador { get; set; }

        public Guid? CursoId { get; set; } // se não for trabalhador
        public Guid? ComissaoTrabalhoId { get; set; } // se for trabalhador

        public TipoCheckin TipoCheckin { get; set; } = TipoCheckin.Evento;
    }
}
