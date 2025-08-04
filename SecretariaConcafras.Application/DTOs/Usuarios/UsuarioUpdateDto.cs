namespace SecretariaConcafras.Application.DTOs.Usuarios
{
    public class UsuarioUpdateDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }

        // Atualizar roles (opcional)
        public ICollection<UsuarioRoleDto>? Roles { get; set; }
    }
}
