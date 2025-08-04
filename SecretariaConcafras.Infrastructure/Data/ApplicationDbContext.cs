using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure.Mappings;
using System.Data;

namespace SecretariaConcafras.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioRole> UsuarioRoles { get; set; }
        public DbSet<UsuarioComissao> UsuarioComissoes { get; set; }

        public DbSet<Participante> Participantes { get; set; }
        public DbSet<Responsavel> Responsaveis { get; set; }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Instituto> Institutos { get; set; }
        public DbSet<ComissaoTrabalho> ComissoesTrabalho { get; set; }

        public DbSet<Inscricao> Inscricoes { get; set; }
        public DbSet<InscricaoCurso> InscricoesCursos { get; set; }
        public DbSet<InscricaoTrabalhador> InscricoesTrabalhadores { get; set; }

        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<PagamentoHistorico> PagamentosHistorico { get; set; }

        public DbSet<Checkin> Checkins { get; set; }

        public DbSet<Instituicao> Instituicoes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Estado> Estados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Estado>().HasData(
                new Estado { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Nome = "São Paulo", Sigla = "SP" },
                new Estado { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Nome = "Minas Gerais", Sigla = "MG" }
            );

            modelBuilder.Entity<Cidade>().HasData(
                // Cidades de SP
                new Cidade { Id = Guid.Parse("aaaa1111-1111-1111-1111-111111111111"), Nome = "São Paulo", EstadoId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                new Cidade { Id = Guid.Parse("aaaa2222-2222-2222-2222-222222222222"), Nome = "Campinas", EstadoId = Guid.Parse("11111111-1111-1111-1111-111111111111") },

                // Cidades de MG
                new Cidade { Id = Guid.Parse("bbbb1111-1111-1111-1111-111111111111"), Nome = "Belo Horizonte", EstadoId = Guid.Parse("22222222-2222-2222-2222-222222222222") },
                new Cidade { Id = Guid.Parse("bbbb2222-2222-2222-2222-222222222222"), Nome = "Uberlândia", EstadoId = Guid.Parse("22222222-2222-2222-2222-222222222222") }
            );


        }
    }
}
