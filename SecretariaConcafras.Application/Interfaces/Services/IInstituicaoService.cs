// Application/Interfaces/Services/IInstituicaoService.cs
namespace SecretariaConcafras.Application.Interfaces.Services
{
    public interface IInstituicaoService
    {
        Task<Guid> ObterOuCriarPorNomeAsync(string nome, CancellationToken ct = default);
    }
}
