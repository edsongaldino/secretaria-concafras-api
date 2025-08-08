using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Eventos;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure;
using System;

namespace SecretariaConcafras.Application.Services.Implementations
{
    public class EventoService : IEventoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EventoService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EventoResponseDto> CriarAsync(EventoCreateDto dto)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            Guid enderecoId;
            if (dto.EnderecoId.HasValue)
            {
                var exists = await _context.Enderecos.AnyAsync(e => e.Id == dto.EnderecoId.Value);
                if (!exists) throw new ArgumentException("EnderecoId informado não existe.");
                enderecoId = dto.EnderecoId.Value;
            }
            else if (dto.Endereco is not null)
            {
                var endereco = _mapper.Map<Endereco>(dto.Endereco);
                _context.Enderecos.Add(endereco);
                await _context.SaveChangesAsync();
                enderecoId = endereco.Id;
            }
            else
            {
                throw new ArgumentException("Informe EnderecoId ou o objeto Endereco.");
            }

            var entity = _mapper.Map<Evento>(dto);
            entity.EnderecoId = enderecoId;
            entity.Endereco = null!;

            _context.Eventos.Add(entity);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();

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
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(e => e.Id == id);

            return entity == null ? null : _mapper.Map<EventoResponseDto>(entity);
        }

        public async Task<IEnumerable<EventoResponseDto>> ObterTodosAsync()
        {
            var entities = await _context.Eventos
                .Include(e => e.Cursos).ThenInclude(c => c.Instituto)
                .Include(e => e.Endereco)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EventoResponseDto>>(entities);
        }
    }
}
