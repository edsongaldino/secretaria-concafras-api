using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Domain.Entities
{
    public class InscricaoTrabalhador
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid InscricaoId { get; set; }
        public Inscricao Inscricao { get; set; }

        public Guid ComissaoTrabalhoId { get; set; }
        public ComissaoTrabalho ComissaoTrabalho { get; set; }

        public NivelTrabalhador Nivel { get; set; } = NivelTrabalhador.Voluntario;
    }
}
