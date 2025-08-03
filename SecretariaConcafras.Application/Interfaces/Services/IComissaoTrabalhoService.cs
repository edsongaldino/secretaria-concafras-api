using SecretariaConcafras.Application.DTOs.Comissoes;

namespace SecretariaConcafras.Application.Interfaces.Services
{
    public interface IComissaoTrabalhoService
    {
        Task<ComissaoTrabalhoResponseDto> CriarAsync(ComissaoTrabalhoCreateDto dto);
        Task<ComissaoTrabalhoResponseDto> AtualizarAsync(ComissaoTrabalhoUpdateDto dto);
        Task<bool> RemoverAsync(Guid id);
        Task<ComissaoTrabalhoResponseDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<ComissaoTrabalhoResponseDto>> ObterTodosAsync();
        Task<IEnumerable<ComissaoTrabalhoResponseDto>> ObterPorEventoAsync(Guid eventoId);
    }
}
