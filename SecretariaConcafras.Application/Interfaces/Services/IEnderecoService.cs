using SecretariaConcafras.Application.DTOs.Enderecos;

namespace SecretariaConcafras.Application.Interfaces.Services
{
    public interface IEnderecoService
    {
        Task<EnderecoResponseDto> CriarAsync(EnderecoCreateDto dto);
        Task<EnderecoResponseDto?> ObterPorIdAsync(Guid id);
        Task<EnderecoResponseDto> AtualizarAsync(EnderecoUpdateDto dto);
        Task<bool> RemoverAsync(Guid id);
    }
}
