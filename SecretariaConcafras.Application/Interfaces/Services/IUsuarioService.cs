using SecretariaConcafras.Application.DTOs;
using SecretariaConcafras.Application.DTOs.Usuarios;

namespace SecretariaConcafras.Application.Interfaces.Services;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioResponseDto>> GetAllAsync();
    Task<UsuarioResponseDto?> GetByIdAsync(Guid id);
    Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto);
    Task<bool> UpdateAsync(Guid id, UsuarioUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<UsuarioResponseDto?> LoginAsync(string email, string senha);
}
