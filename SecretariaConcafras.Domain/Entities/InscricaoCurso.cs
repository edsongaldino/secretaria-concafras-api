using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Domain.Entities
{
    public class InscricaoCurso
    {
        // PK composta
        public Guid InscricaoId { get; set; }
        public Guid CursoId { get; set; }

        // ÚNICAS navegações!
        public Inscricao Inscricao { get; set; } = default!;
        public Curso Curso { get; set; } = default!;
    }
}
