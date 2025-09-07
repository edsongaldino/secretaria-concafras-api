// Application/Services/Implementations/InstituicaoService.cs
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.Interfaces;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure;
using System.Globalization;
using System.Text;

public class InstituicaoService : IInstituicaoService
{
    private readonly ApplicationDbContext _db;
    public InstituicaoService(ApplicationDbContext db) => _db = db;

    public async Task<Guid> ObterOuCriarPorNomeAsync(string nome, CancellationToken ct = default)
    {
        var norm = Normalizar(nome);

        var existente = await _db.Instituicoes.AsNoTracking()
            .FirstOrDefaultAsync(i => i.NomeNormalizado == norm, ct);
        if (existente != null) return existente.Id;

        var inst = new Instituicao { Id = Guid.NewGuid(), Nome = nome.Trim(), NomeNormalizado = norm };
        _db.Instituicoes.Add(inst);

        try
        {
            await _db.SaveChangesAsync(ct);
            return inst.Id;
        }
        catch (DbUpdateException)
        {
            var again = await _db.Instituicoes.AsNoTracking()
                .FirstOrDefaultAsync(i => i.NomeNormalizado == norm, ct);
            if (again != null) return again.Id;
            throw;
        }
    }

    static string Normalizar(string v)
    {
        var s = (v ?? "").Trim().ToUpperInvariant();
        var formD = s.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(formD.Length);
        foreach (var ch in formD)
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark) sb.Append(ch);
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
