using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Inscricoes
{
    public class InscricaoResponseDto
    {
        public Guid Id { get; set; }
        public string ParticipanteNome { get; set; }
        public string EventoTitulo { get; set; }

        public bool EhTrabalhador { get; set; }
        public string? CursoTitulo { get; set; }
        public string? ComissaoNome { get; set; }

        public TipoCheckin TipoCheckin { get; set; }
        public DateTime DataInscricao { get; set; }
    }
}
