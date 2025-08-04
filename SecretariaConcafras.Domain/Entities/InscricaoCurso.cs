using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Domain.Entities
{
    public class InscricaoCurso
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid InscricaoId { get; set; }
        public Inscricao Inscricao { get; set; }

        public Guid CursoId { get; set; }
        public Curso Curso { get; set; }

        // Caso o curso seja por blocos
        public BlocoCurso TipoBloco { get; set; } = BlocoCurso.TemaAtual;
    }
}
