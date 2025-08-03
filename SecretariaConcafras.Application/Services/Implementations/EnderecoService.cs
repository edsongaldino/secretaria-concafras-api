using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Cidades;
using SecretariaConcafras.Application.DTOs.Enderecos;
using SecretariaConcafras.Application.DTOs.Estados;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure.Context;
using System;

namespace SecretariaConcafras.Application.Services.Implementations
{
    public class EstadoService : IEstadoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EstadoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EstadoResponseDto>> ObterTodosAsync()
        {
            var estados = await _context.Estados.OrderBy(e => e.Nome).ToListAsync();
            return _mapper.Map<IEnumerable<EstadoResponseDto>>(estados);
        }
    }

    public class CidadeService : ICidadeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CidadeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CidadeResponseDto>> ObterPorEstadoAsync(Guid estadoId)
        {
            var cidades = await _context.Cidades
                .Where(c => c.EstadoId == estadoId)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CidadeResponseDto>>(cidades);
        }
    }

    public class EnderecoService : IEnderecoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EnderecoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EnderecoResponseDto> CriarAsync(EnderecoCreateDto dto)
        {
            var entity = _mapper.Map<Endereco>(dto);
            _context.Enderecos.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<EnderecoResponseDto>(entity);
        }

        public async Task<EnderecoResponseDto?> ObterPorIdAsync(Guid id)
        {
            var entity = await _context.Enderecos
                .Include(e => e.Cidade)
                    .ThenInclude(c => c.Estado)
                .FirstOrDefaultAsync(e => e.Id == id);

            return entity == null ? null : _mapper.Map<EnderecoResponseDto>(entity);
        }

        public async Task<EnderecoResponseDto> AtualizarAsync(EnderecoUpdateDto dto)
        {
            var entity = await _context.Enderecos.FindAsync(dto.Id);
            if (entity == null) throw new KeyNotFoundException("Endereço não encontrado.");

            _mapper.Map(dto, entity);
            _context.Enderecos.Update(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<EnderecoResponseDto>(entity);
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            var entity = await _context.Enderecos.FindAsync(id);
            if (entity == null) return false;

            _context.Enderecos.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
