using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Domain.Enums;

public class InscricaoTrabalhador
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid InscricaoId { get; set; }
    public Inscricao Inscricao { get; set; } = default!;

    public Guid ComissaoEventoId { get; set; }
    public ComissaoEvento ComissaoEvento { get; set; } = default!;

    public NivelTrabalhador Nivel { get; set; }
}
