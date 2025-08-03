using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Cursos
{
    public class CursoUpdateDto : CursoCreateDto
    {
        public Guid Id { get; set; }
    }
}
