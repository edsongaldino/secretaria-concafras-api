// Application/Services/Implementations/ParticipanteService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Participantes;
using SecretariaConcafras.Application.Interfaces;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure;
using System.Linq;

public class ParticipanteService : IParticipanteService
{
    private readonly ApplicationDbContext _db;
    private readonly IInstituicaoService _instituicaoSvc;
    private readonly IMapper _mapper;

    public ParticipanteService(ApplicationDbContext db, IInstituicaoService instituicaoSvc, IMapper mapper)
    {
        _db = db;
        _instituicaoSvc = instituicaoSvc;
        _mapper = mapper;
    }

    public async Task<ParticipanteResultadoDto> UpsertPorCpfAsync(ParticipanteCreateDto dto, CancellationToken ct = default)
    {
        var cpf = OnlyDigits(dto.Cpf);
        if (cpf.Length != 11) throw new ArgumentException("CPF inválido.", nameof(dto.Cpf));

        Guid? instituicaoId = null;
        if (!string.IsNullOrWhiteSpace(dto.Instituicao))
            instituicaoId = await _instituicaoSvc.ObterOuCriarPorNomeAsync(dto.Instituicao!, ct);

        var existente = await _db.Participantes
            .Include(p => p.Endereco)
            .FirstOrDefaultAsync(p => p.CPF == cpf, ct);

        if (existente is null)
        {
            var novo = _mapper.Map<Participante>(dto);
            novo.CPF = cpf;
            novo.InstituicaoId = instituicaoId;
            if (!dto.DataNascimento.HasValue) novo.DataNascimento = DateTime.MinValue; // se sua entidade exige

            _db.Participantes.Add(novo);
            await _db.SaveChangesAsync(ct);
            return new ParticipanteResultadoDto(novo.Id, novo.Nome);
        }
        else
        {
            _mapper.Map(dto, existente); // atualiza só campos não-nulos
            existente.CPF = cpf;
            if (instituicaoId.HasValue) existente.InstituicaoId = instituicaoId;

            await _db.SaveChangesAsync(ct);
            return new ParticipanteResultadoDto(existente.Id, existente.Nome);
        }
    }

    static string OnlyDigits(string v) => new string((v ?? "").Where(char.IsDigit).ToArray());
}
