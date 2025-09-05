// Application/DTOs/Comissoes/ComissaoEventoDtos.cs
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

    public class ComissoesParaEventoCreateDto
    {
        public Guid EventoId { get; set; }
        public IEnumerable<Guid> ComissaoIds { get; set; } = Array.Empty<Guid>();
        public IEnumerable<CoordenadorVinculoDto>? Coordenadores { get; set; }
    }

    public class CoordenadorVinculoDto
    {
        public Guid ComissaoId { get; set; }
        public Guid UsuarioId { get; set; }
    }

    public class ComissaoEventoUpdateDto
    {
        public string? Observacoes { get; set; }
        public Guid? CoordenadorUsuarioId { get; set; }
    }
}
