using SecretariaConcafras.Application.DTOs.Inscricoes;

namespace SecretariaConcafras.Application.Interfaces.Services
{
    public interface IInscricaoService
    {
        Task<InscricaoResponseDto> CriarAsync(InscricaoCreateDto dto);
        Task<bool> CancelarAsync(Guid id);
        Task<InscricaoResponseDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<InscricaoResponseDto>> ObterPorEventoAsync(Guid eventoId);
        Task<IEnumerable<InscricaoResponseDto>> ObterPorParticipanteAsync(Guid participanteId);
    }
}
