using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Cursos
{
    public class CursoResponseDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public int Vagas { get; set; }
        public BlocoCurso Bloco { get; set; }

        // Nome do evento ao qual o curso pertence
        public string EventoTitulo { get; set; }

        // Nome do instituto responsável pelo curso
        public string InstitutoNome { get; set; }

    }
}
