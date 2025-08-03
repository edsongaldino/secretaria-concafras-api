using System.ComponentModel.DataAnnotations;
using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Domain.Entities
{
    public class Curso
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(150)]
        public string Titulo { get; set; }

        [MaxLength(500)]
        public string? Descricao { get; set; }

        [Required]
        public BlocoCurso Bloco { get; set; }

        [Required]
        public int Vagas { get; set; }

        // Relacionamento com Evento
        public Guid EventoId { get; set; }
        public Evento Evento { get; set; }

        // Relacionamento com Instituto
        public Guid InstitutoId { get; set; }
        public Instituto Instituto { get; set; }

        // Participantes inscritos neste curso
        public ICollection<InscricaoCurso> InscricoesCurso { get; set; } = new List<InscricaoCurso>();
    }
}
