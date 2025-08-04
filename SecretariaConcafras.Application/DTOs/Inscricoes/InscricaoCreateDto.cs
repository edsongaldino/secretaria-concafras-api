using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Inscricoes
{
    public class InscricaoCreateDto
    {
        public Guid ParticipanteId { get; set; }
        public Guid EventoId { get; set; }

        // Cursos escolhidos (até 2)
        public List<Guid> CursosIds { get; set; } = new();

        // Caso seja trabalhador
        public Guid? ComissaoTrabalhoId { get; set; }
    }
}
