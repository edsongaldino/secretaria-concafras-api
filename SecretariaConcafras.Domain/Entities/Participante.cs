using SecretariaConcafras.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class Participante
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(150)]
    public string Nome { get; set; }

    [Required]
    public DateTime DataNascimento { get; set; }

    [Required, MaxLength(11)] // 11 dígitos do CPF
    public string CPF { get; set; }

    // Vínculo com usuário (quando aplicável)
    public Usuario? Usuario { get; set; }

    // Instituição de origem
    public Guid? InstituicaoId { get; set; }
    public Instituicao? Instituicao { get; set; }

    // Endereço
    public Guid? EnderecoId { get; set; }
    public Endereco? Endereco { get; set; }

    // Responsável (caso menor de idade)
    public Guid? ResponsavelId { get; set; }
    public Participante? Responsavel { get; set; }

    // Inscrições em eventos
    public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();
}
