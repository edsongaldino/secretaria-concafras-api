using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Eventos;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure.Context;
using System;

namespace SecretariaConcafras.Application.Services.Implementations
{
    public class EventoService : IEventoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EventoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EventoResponseDto> CriarAsync(EventoCreateDto dto)
        {
            var entity = _mapper.Map<Evento>(dto);
            _context.Eventos.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<EventoResponseDto>(entity);
        }

        public async Task<EventoResponseDto> AtualizarAsync(EventoUpdateDto dto)
        {
            var entity = await _context.Eventos.FindAsync(dto.Id);
            if (entity == null) throw new KeyNotFoundException("Evento não encontrado.");

            _mapper.Map(dto, entity);
            _context.Eventos.Update(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<EventoResponseDto>(entity);
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            var entity = await _context.Eventos.FindAsync(id);
            if (entity == null) return false;

            _context.Eventos.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EventoResponseDto?> ObterPorIdAsync(Guid id)
        {
            var entity = await _context.Eventos
                .Include(e => e.Cursos).ThenInclude(c => c.Instituto)
                .Include(e => e.Endereco).ThenInclude(en => en.Cidade).ThenInclude(c => c.Estado)
                .FirstOrDefaultAsync(e => e.Id == id);

            return entity == null ? null : _mapper.Map<EventoResponseDto>(entity);
        }

        public async Task<IEnumerable<EventoResponseDto>> ObterTodosAsync()
        {
            var entities = await _context.Eventos
                .Include(e => e.Cursos).ThenInclude(c => c.Instituto)
                .Include(e => e.Endereco).ThenInclude(en => en.Cidade).ThenInclude(c => c.Estado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EventoResponseDto>>(entities);
        }
    }
}
