using SecretariaConcafras.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SecretariaConcafras.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
}
