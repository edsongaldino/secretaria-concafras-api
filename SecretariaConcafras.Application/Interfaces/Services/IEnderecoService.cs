using SecretariaConcafras.Application.DTOs.Estados;
using SecretariaConcafras.Application.DTOs.Cidades;
using SecretariaConcafras.Application.DTOs.Enderecos;

namespace SecretariaConcafras.Application.Interfaces.Services
{
    public interface IEstadoService
    {
        Task<IEnumerable<EstadoResponseDto>> ObterTodosAsync();
    }

    public interface ICidadeService
    {
        Task<IEnumerable<CidadeResponseDto>> ObterPorEstadoAsync(Guid estadoId);
    }

    public interface IEnderecoService
    {
        Task<EnderecoResponseDto> CriarAsync(EnderecoCreateDto dto);
        Task<EnderecoResponseDto?> ObterPorIdAsync(Guid id);
        Task<EnderecoResponseDto> AtualizarAsync(EnderecoUpdateDto dto);
        Task<bool> RemoverAsync(Guid id);
    }
}
