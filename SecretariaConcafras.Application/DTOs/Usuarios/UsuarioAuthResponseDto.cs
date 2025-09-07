namespace SecretariaConcafras.Application.DTOs.Usuarios
{
    public class UsuarioAuthResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }

        public ICollection<UsuarioRoleDto> Roles { get; set; } = new List<UsuarioRoleDto>();
    }
}
