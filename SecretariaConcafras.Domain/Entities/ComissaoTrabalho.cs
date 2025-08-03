namespace SecretariaConcafras.Domain.Entities
{
    public class ComissaoTrabalho
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Nome { get; set; }

        // Vínculo com evento
        public Guid EventoId { get; set; }
        public Evento Evento { get; set; }

        // Participantes que atuam nessa comissão
        public ICollection<InscricaoTrabalhador> Trabalhadores { get; set; } = new List<InscricaoTrabalhador>();

        // Usuários vinculados com perfis de acesso
        public ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();
    }
}
