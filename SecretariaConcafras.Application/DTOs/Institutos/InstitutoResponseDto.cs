using SecretariaConcafras.Application.DTOs.Cursos;

namespace SecretariaConcafras.Application.DTOs.Institutos
{
    public class InstitutoResponseDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public ICollection<CursoResponseDto>? Cursos { get; set; }
    }
}
