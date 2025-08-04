using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Domain.Interfaces;
using SecretariaConcafras.Infrastructure;

namespace SecretariaConcafras.Infrastructure.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObterPorEmailSenhaAsync(string email, string senha)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
        }
    }
}
