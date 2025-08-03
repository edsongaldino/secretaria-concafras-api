using SecretariaConcafras.Application.DTOs.Pagamentos;

namespace SecretariaConcafras.Application.Interfaces
{
    public interface IPagamentoService
    {
        Task<PagamentoResponseDto> CriarAsync(PagamentoCreateDto dto);
        Task<PagamentoResponseDto?> AtualizarAsync(Guid id, PagamentoUpdateDto dto);
        Task<bool> DeletarAsync(Guid id);
        Task<PagamentoResponseDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<PagamentoResponseDto>> ObterTodosAsync();
        Task<IEnumerable<PagamentoResponseDto>> ObterPorInscricaoAsync(Guid inscricaoId);
    }
}
