using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Cursos
{
    public class CursoCreateDto
    {
        public string Titulo { get; set; }
        public int Vagas { get; set; }
        public BlocoCurso Bloco { get; set; }

        public Guid EventoId { get; set; }
        public Guid InstitutoId { get; set; }
    }
}
