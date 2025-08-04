using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Usuarios;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Domain.Interfaces;

namespace SecretariaConcafras.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IRepository<Usuario> _repository;
        private readonly IMapper _mapper;

        public UsuarioService(IRepository<Usuario> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UsuarioResponseDto>> GetAllAsync()
        {
            var usuarios = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<UsuarioResponseDto>>(usuarios);
        }

        public async Task<UsuarioResponseDto?> GetByIdAsync(Guid id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            return _mapper.Map<UsuarioResponseDto?>(usuario);
        }

        public async Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto)
        {
            var usuario = _mapper.Map<Usuario>(dto);
            await _repository.AddAsync(usuario);
            return _mapper.Map<UsuarioResponseDto>(usuario);
        }

        public async Task<bool> UpdateAsync(Guid id, UsuarioUpdateDto dto)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null) return false;

            _mapper.Map(dto, usuario);
            await _repository.UpdateAsync(usuario);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null) return false;

            await _repository.DeleteAsync(usuario);
            return true;
        }

        public async Task<UsuarioResponseDto?> LoginAsync(string email, string senha)
        {
            var usuarios = await _repository.FindAsync(u => u.Email == email && u.Senha == senha);
            var usuario = usuarios.FirstOrDefault();
            return _mapper.Map<UsuarioResponseDto?>(usuario);
        }
    }
}
