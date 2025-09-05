using SecretariaConcafras.Application.DTOs.Comissoes;

namespace SecretariaConcafras.Application.Interfaces.Services
{
    public interface IComissaoEventoService
    {
        // cria N vínculos de uma vez (comissões do catálogo → evento)
        Task CriarParaEventoAsync(ComissoesParaEventoCreateDto dto);

        // leitura / listagem
        Task<ComissaoEventoDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<ComissaoEventoDto>> ObterPorEventoAsync(Guid eventoId);

        // atualizações
        Task AtualizarAsync(Guid id, ComissaoEventoUpdateDto dto);
        Task AtualizarCoordenadorAsync(Guid id, Guid? usuarioId);

        // remoção
        Task<bool> RemoverAsync(Guid id);
    }
}
