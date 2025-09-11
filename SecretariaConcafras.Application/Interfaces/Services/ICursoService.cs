using SecretariaConcafras.Application.DTOs.Cursos;
using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.Interfaces.Services
{
    public interface ICursoService
    {
        Task<CursoResponseDto> CriarAsync(CursoCreateDto dto);
        Task<CursoResponseDto> AtualizarAsync(CursoUpdateDto dto);
        Task<bool> RemoverAsync(Guid id);
        Task<CursoResponseDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<CursoResponseDto>> ObterTodosAsync();
        Task<IEnumerable<CursoResponseDto>> ObterPorEventoAsync(Guid eventoId, PublicoCurso? publico, BlocoCurso? bloco, bool? neofito);
    }
}
