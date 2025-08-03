using SecretariaConcafras.Application.DTOs.Participantes;

namespace SecretariaConcafras.Application.Interfaces.Services
{
    public interface IParticipanteService
    {
        Task<ParticipanteResponseDto> CriarAsync(ParticipanteCreateDto dto);
        Task<ParticipanteResponseDto> AtualizarAsync(ParticipanteUpdateDto dto);
        Task<bool> RemoverAsync(Guid id);
        Task<ParticipanteResponseDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<ParticipanteResponseDto>> ObterTodosAsync();
    }
}
