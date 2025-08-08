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
        
    }
}
