using ProjetoBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProjetoBase.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
}