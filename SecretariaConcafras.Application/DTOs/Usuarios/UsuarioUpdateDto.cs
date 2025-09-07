namespace SecretariaConcafras.Application.DTOs.Usuarios
{
    public class UsuarioUpdateDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        // Atualizar roles (opcional)
        public ICollection<UsuarioRoleDto>? Roles { get; set; }
    }
}
