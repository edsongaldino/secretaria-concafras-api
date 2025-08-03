using SecretariaConcafras.Application.DTOs.Eventos;

namespace SecretariaConcafras.Application.Interfaces.Services
{
    public interface IEventoService
    {
        Task<EventoResponseDto> CriarAsync(EventoCreateDto dto);
        Task<EventoResponseDto> AtualizarAsync(EventoUpdateDto dto);
        Task<bool> RemoverAsync(Guid id);
        Task<EventoResponseDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<EventoResponseDto>> ObterTodosAsync();
    }
}
