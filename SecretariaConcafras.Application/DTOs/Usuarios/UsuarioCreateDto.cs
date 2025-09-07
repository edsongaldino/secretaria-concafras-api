namespace SecretariaConcafras.Application.DTOs.Usuarios
{
    public class UsuarioCreateDto
    {
        public string Email { get; set; }
        public string Senha { get; set; }  // Vai ser criptografada no Service
        public ICollection<UsuarioRoleDto> Roles { get; set; } = new List<UsuarioRoleDto>();
    }
}
