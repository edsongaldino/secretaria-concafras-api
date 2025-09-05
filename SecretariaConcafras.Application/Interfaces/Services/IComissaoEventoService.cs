using SecretariaConcafras.Application.DTOs.Comissoes;

namespace SecretariaConcafras.Application.Interfaces.Services
{
    public interface IComissaoEventoService
    {
		Task<IEnumerable<ComissaoEventoListItemDto>> ListarAsync(Guid eventoId, string? q = null, int skip = 0, int take = 100);
		Task<IEnumerable<ComissaoEventoCountsDto>> ContagensAsync(Guid eventoId);
		Task<IEnumerable<InscricaoTrabalhadorSlimDto>> ListarInscricoesAsync(Guid comissaoEventoId);
		Task<IEnumerable<UsuarioRoleSlimDto>> ListarUsuariosAsync(Guid comissaoEventoId);
	}
}
