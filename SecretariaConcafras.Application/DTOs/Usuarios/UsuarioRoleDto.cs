using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Usuarios
{
    public class UsuarioRoleDto
    {
        public RoleSistema Role { get; set; }
        public Guid? EventoId { get; set; }
        public Guid? ComissaoTrabalhoId { get; set; }
    }
}
