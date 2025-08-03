using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Domain.Entities
{
    public class UsuarioRole
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public RoleSistema Role { get; set; }

        // Roles podem ser globais ou vinculadas a um evento/comissão
        public Guid? EventoId { get; set; }
        public Evento? Evento { get; set; }

        public Guid? ComissaoTrabalhoId { get; set; }
        public ComissaoTrabalho? ComissaoTrabalho { get; set; }
    }
}
