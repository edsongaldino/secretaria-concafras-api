using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Cursos
{
    public class CursoResponseDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public int Vagas { get; set; }
        public BlocoCurso Bloco { get; set; }

        public string EventoTitulo { get; set; }
        public string InstitutoNome { get; set; }
    }
}
