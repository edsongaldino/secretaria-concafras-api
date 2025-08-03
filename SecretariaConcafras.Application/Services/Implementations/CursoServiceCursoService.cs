using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Cursos;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure.Context;
using System;

namespace SecretariaConcafras.Application.Services.Implementations
{
    public class CursoService : ICursoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CursoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CursoResponseDto> CriarAsync(CursoCreateDto dto)
        {
            var entity = _mapper.Map<Curso>(dto);
            _context.Cursos.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<CursoResponseDto>(entity);
        }

        public async Task<CursoResponseDto> AtualizarAsync(CursoUpdateDto dto)
        {
            var entity = await _context.Cursos.FindAsync(dto.Id);
            if (entity == null) throw new KeyNotFoundException("Curso não encontrado.");

            _mapper.Map(dto, entity);
            _context.Cursos.Update(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<CursoResponseDto>(entity);
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            var entity = await _context.Cursos.FindAsync(id);
            if (entity == null) return false;

            _context.Cursos.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CursoResponseDto?> ObterPorIdAsync(Guid id)
        {
            var entity = await _context.Cursos
                .Include(c => c.Evento)
                .Include(c => c.Instituto)
                .FirstOrDefaultAsync(c => c.Id == id);

            return entity == null ? null : _mapper.Map<CursoResponseDto>(entity);
        }

        public async Task<IEnumerable<CursoResponseDto>> ObterTodosAsync()
        {
            var entities = await _context.Cursos
                .Include(c => c.Evento)
                .Include(c => c.Instituto)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CursoResponseDto>>(entities);
        }

        public async Task<IEnumerable<CursoResponseDto>> ObterPorEventoAsync(Guid eventoId)
        {
            var entities = await _context.Cursos
                .Include(c => c.Evento)
                .Include(c => c.Instituto)
                .Where(c => c.EventoId == eventoId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CursoResponseDto>>(entities);
        }
    }
}
