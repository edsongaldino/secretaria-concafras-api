using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Usuarios
{
    public class UsuarioResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public ICollection<UsuarioRoleDto> Roles { get; set; } = new List<UsuarioRoleDto>();
    }
}
