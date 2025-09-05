using SecretariaConcafras.Application.DTOs.Comissoes;

namespace SecretariaConcafras.Application.Interfaces.Services
{
    public interface IComissaoService
    {
        Task<Guid> CriarAsync(ComissaoCreateDto dto);
        Task AtualizarAsync(Guid id, ComissaoUpdateDto dto);
        Task<bool> RemoverAsync(Guid id);
        Task<ComissaoDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<ComissaoDto>> ObterTodosAsync();
    }
}
