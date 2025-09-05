using SecretariaConcafras.Application.DTOs.Comissoes;
using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.DTOs.Usuarios
{
    public class UsuarioRoleDto
    {
		public UsuarioRoleDto(Guid id, Guid usuarioId, RoleSistema role, Guid? eventoId, Guid? comissaoEventoId, UsuarioSlimDto usuarioSlimDto)
		{
			Id = id;
			EventoId = eventoId;
			ComissaoEventoId = comissaoEventoId;
		}

		public Guid Id { get; set; }
        public int Role { get; set; }
        public Guid? EventoId { get; set; }
        public Guid? ComissaoEventoId { get; set; }
        public UsuarioRoleDto Usuario { get; set; }
	}
}