using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Comissoes;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure;

namespace SecretariaConcafras.Application.Services.Implementations
{
    public class ComissaoTrabalhoService : IComissaoTrabalhoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ComissaoTrabalhoService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ComissaoTrabalhoResponseDto> CriarAsync(ComissaoTrabalhoCreateDto dto)
        {
            var entity = _mapper.Map<ComissaoTrabalho>(dto);
            _context.ComissoesTrabalho.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ComissaoTrabalhoResponseDto>(entity);
        }

        public async Task<ComissaoTrabalhoResponseDto> AtualizarAsync(ComissaoTrabalhoUpdateDto dto)
        {
            var entity = await _context.ComissoesTrabalho.FindAsync(dto.Id);
            if (entity == null) throw new KeyNotFoundException("Comissão não encontrada.");

            _mapper.Map(dto, entity);
            _context.ComissoesTrabalho.Update(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<ComissaoTrabalhoResponseDto>(entity);
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            var entity = await _context.ComissoesTrabalho.FindAsync(id);
            if (entity == null) return false;

            _context.ComissoesTrabalho.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ComissaoTrabalhoResponseDto?> ObterPorIdAsync(Guid id)
        {
            var entity = await _context.ComissoesTrabalho
                .Include(c => c.Evento)
                .Include(c => c.Trabalhadores)
                    .ThenInclude(t => t.Inscricao)
                .Include(c => c.UsuarioRoles)
                    .ThenInclude(ur => ur.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);

            return entity == null ? null : _mapper.Map<ComissaoTrabalhoResponseDto>(entity);
        }

        public async Task<IEnumerable<ComissaoTrabalhoResponseDto>> ObterTodosAsync()
        {
            var entities = await _context.ComissoesTrabalho
                .Include(c => c.Evento)
                .Include(c => c.Trabalhadores)
                    .ThenInclude(t => t.Inscricao)
                .Include(c => c.UsuarioRoles)
                    .ThenInclude(ur => ur.Usuario)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ComissaoTrabalhoResponseDto>>(entities);
        }

        public async Task<IEnumerable<ComissaoTrabalhoResponseDto>> ObterPorEventoAsync(Guid eventoId)
        {
            var entities = await _context.ComissoesTrabalho
                .Include(c => c.Evento)
                .Include(c => c.Trabalhadores)
                    .ThenInclude(t => t.Inscricao)
                .Include(c => c.UsuarioRoles)
                    .ThenInclude(ur => ur.Usuario)
                .Where(c => c.EventoId == eventoId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ComissaoTrabalhoResponseDto>>(entities);
        }

    }
}
