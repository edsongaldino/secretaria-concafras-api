// Application/DTOs/Comissoes/ComissaoEventoDtos.cs
using SecretariaConcafras.Application.DTOs.Inscricoes;
using SecretariaConcafras.Application.DTOs.Usuarios;

namespace SecretariaConcafras.Application.DTOs.Comissoes
{
	public class ComissaoEventoDto
	{
		public Guid Id { get; set; }
		public Guid EventoId { get; set; }
		public Guid ComissaoId { get; set; }

		public ComissaoDto Comissao { get; set; } = default!;

		public Guid? CoordenadorUsuarioId { get; set; }
		public UsuarioSlimDto? CoordenadorUsuario { get; set; }

		public string? Observacoes { get; set; }

		public List<InscricaoTrabalhadorSlimDto> InscricoesTrabalhadores { get; set; } = new();
		public List<UsuarioRoleSlimDto> UsuarioRoles { get; set; } = new();
	}

	public class ComissaoEventoListItemDto
	{
		public Guid Id { get; set; }
		public Guid EventoId { get; set; }
		public Guid ComissaoId { get; set; }
		public string ComissaoNome { get; set; } = default!;
		public string? Observacoes { get; set; } // opcional
	}

	// (opcional) para badges no front sem carregar coleções
	public class ComissaoEventoCountsDto
	{
		public Guid ComissaoEventoId { get; set; }
		public int QtdInscritosTrabalhador { get; set; }
		public int QtdUsuarioRoles { get; set; }
	}
	public class ComissaoEventoUpdateDto
	{
		public string? Observacoes { get; set; }
		public Guid? CoordenadorUsuarioId { get; set; }
	}
}
