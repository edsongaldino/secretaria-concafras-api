using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Participantes;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure;
using System;

namespace SecretariaConcafras.Application.Services.Implementations
{
    public class ParticipanteService : IParticipanteService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ParticipanteService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ParticipanteResponseDto> CriarAsync(ParticipanteCreateDto dto)
        {
            var entity = _mapper.Map<Participante>(dto);
            _context.Participantes.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ParticipanteResponseDto>(entity);
        }

        public async Task<ParticipanteResponseDto> AtualizarAsync(ParticipanteUpdateDto dto)
        {
            var entity = await _context.Participantes.FindAsync(dto.Id);
            if (entity == null) throw new KeyNotFoundException("Participante não encontrado.");

            _mapper.Map(dto, entity);
            _context.Participantes.Update(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ParticipanteResponseDto>(entity);
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            var entity = await _context.Participantes.FindAsync(id);
            if (entity == null) return false;

            _context.Participantes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ParticipanteResponseDto?> ObterPorIdAsync(Guid id)
        {
            var entity = await _context.Participantes
                .Include(p => p.Responsavel)
                .Include(p => p.Instituicao)
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(p => p.Id == id);

            return entity == null ? null : _mapper.Map<ParticipanteResponseDto>(entity);
        }

        public async Task<IEnumerable<ParticipanteResponseDto>> ObterTodosAsync()
        {
            var entities = await _context.Participantes
                .Include(p => p.Responsavel)
                .Include(p => p.Instituicao)
                .Include(p => p.Endereco)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ParticipanteResponseDto>>(entities);
        }
    }
}
