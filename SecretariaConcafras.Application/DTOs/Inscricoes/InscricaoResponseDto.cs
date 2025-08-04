using SecretariaConcafras.Application.DTOs.Cursos;
using SecretariaConcafras.Application.DTOs.Comissoes;
using SecretariaConcafras.Application.DTOs.Checkins;

namespace SecretariaConcafras.Application.DTOs.Inscricoes
{
    public class InscricaoResponseDto
    {
        public Guid Id { get; set; }

        public string ParticipanteNome { get; set; }
        public string EventoTitulo { get; set; }

        // Se é trabalhador voluntário
        public bool EhTrabalhador { get; set; }

        // Cursos da inscrição
        public List<CursoResponseDto> Cursos { get; set; } = new();

        // Comissão do trabalhador (se aplicável)
        public ComissaoTrabalhoResponseDto? Comissao { get; set; }

        public DateTime DataInscricao { get; set; }

        // Histórico de check-ins
        public List<CheckinResponseDto> Checkins { get; set; } = new();
    }
}
