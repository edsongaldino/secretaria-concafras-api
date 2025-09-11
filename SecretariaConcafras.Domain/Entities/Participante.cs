using SecretariaConcafras.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class Participante
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(150)]
    public string Nome { get; set; }

    [Required]
    public DateOnly DataNascimento { get; set; }

    [Required, MaxLength(11)] // 11 d�gitos do CPF
    public string CPF { get; set; }

    // V�nculo com usu�rio (quando aplic�vel)
    public Usuario? Usuario { get; set; }

    // Institui��o de origem
    public Guid? InstituicaoId { get; set; }
    public Instituicao? Instituicao { get; set; }

    // Endere�o
    public Guid? EnderecoId { get; set; }
    public Endereco? Endereco { get; set; }


    // Inscri��es em eventos
    public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();
}
