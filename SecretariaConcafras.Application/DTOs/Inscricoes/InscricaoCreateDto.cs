using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Inscricoes
{
    public class InscricaoCreateDto
    {
        public Guid EventoId { get; set; }
        public Guid ParticipanteId { get; set; }
        public Guid? ResponsavelFinanceiroId { get; set; }        
        public Guid? CursoTemaAtualId { get; set; }
        public Guid? CursoTemaEspecificoId { get; set; }
        public Guid? ComissaoId { get; set; }
    }

    public class InscricaoCreateResultDto
    {
        public Guid Id { get; init; }
        public bool Trabalhador { get; init; }   // inferido
        public string Message { get; init; } = "Inscrição criada";
    }
}
