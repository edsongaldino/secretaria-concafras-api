using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets podem ficar como estão
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioRole> UsuarioRoles { get; set; }
        public DbSet<UsuarioComissao> UsuarioComissoes { get; set; }

        public DbSet<Participante> Participantes { get; set; }
        public DbSet<Responsavel> Responsaveis { get; set; }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Instituto> Institutos { get; set; }
        public DbSet<ComissaoEvento> ComissoesTrabalho { get; set; }

        public DbSet<Inscricao> Inscricoes { get; set; }
        public DbSet<InscricaoCurso> InscricoesCursos { get; set; }
        public DbSet<InscricaoTrabalhador> InscricoesTrabalhadores { get; set; }

        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<PagamentoItem> PagamentoItens { get; set; }

        public DbSet<Checkin> Checkins { get; set; }

        public DbSet<Instituicao> Instituicoes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // schema padrão do seu DDL
            modelBuilder.HasDefaultSchema("public");

            // aplica TODAS as classes que implementam IEntityTypeConfiguration<>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            // Se os mappings estiverem em outro assembly/projeto, aponte por um tipo de lá:
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(CursoMapping).Assembly);
        }
    }
}
