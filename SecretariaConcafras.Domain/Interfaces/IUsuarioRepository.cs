using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        // Aqui ficam apenas métodos específicos de usuário
        Task<Usuario?> ObterPorEmailSenhaAsync(string email, string senha);
    }
}
