using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Inscricoes;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure;
using System;

namespace SecretariaConcafras.Application.Services.Implementations
{
    public class InscricaoService : IInscricaoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public InscricaoService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InscricaoResponseDto> CriarAsync(InscricaoCreateDto dto)
        {
            var entity = _mapper.Map<Inscricao>(dto);
            entity.DataInscricao = DateTime.UtcNow;

            _context.Inscricoes.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<InscricaoResponseDto>(entity);
        }

        public async Task<bool> CancelarAsync(Guid id)
        {
            var entity = await _context.Inscricoes.FindAsync(id);
            if (entity == null) return false;

            _context.Inscricoes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<InscricaoResponseDto?> ObterPorIdAsync(Guid id)
        {
            var entity = await _context.Inscricoes
                .Include(i => i.Participante)
                .Include(i => i.Evento)
                .Include(i => i.InscricaoCurso)
                .Include(i => i.InscricaoTrabalhador)
                .FirstOrDefaultAsync(i => i.Id == id);

            return entity == null ? null : _mapper.Map<InscricaoResponseDto>(entity);
        }

        public async Task<IEnumerable<InscricaoResponseDto>> ObterPorEventoAsync(Guid eventoId)
        {
            var entities = await _context.Inscricoes
                .Include(i => i.Participante)
                .Include(i => i.Evento)
                .Include(i => i.InscricaoCurso)
                .Include(i => i.InscricaoTrabalhador)
                .Where(i => i.EventoId == eventoId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InscricaoResponseDto>>(entities);
        }

        public async Task<IEnumerable<InscricaoResponseDto>> ObterPorParticipanteAsync(Guid participanteId)
        {
            var entities = await _context.Inscricoes
                .Include(i => i.Participante)
                .Include(i => i.Evento)
                .Include(i => i.InscricaoCurso)
                .Include(i => i.InscricaoTrabalhador)
                .Where(i => i.ParticipanteId == participanteId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InscricaoResponseDto>>(entities);
        }
    }
}
