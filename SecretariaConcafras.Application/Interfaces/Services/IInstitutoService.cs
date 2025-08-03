using SecretariaConcafras.Application.DTOs.Institutos;

namespace SecretariaConcafras.Application.Interfaces
{
    public interface IInstitutoService
    {
        Task<InstitutoResponseDto> CriarAsync(InstitutoCreateDto dto);
        Task<InstitutoResponseDto?> AtualizarAsync(Guid id, InstitutoUpdateDto dto);
        Task<bool> DeletarAsync(Guid id);
        Task<InstitutoResponseDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<InstitutoResponseDto>> ObterTodosAsync();
    }
}
