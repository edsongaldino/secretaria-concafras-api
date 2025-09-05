// ComissaoTrabalho.cs
using SecretariaConcafras.Domain.Entities;

public class ComissaoEvento
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid EventoId { get; set; }
    public Evento Evento { get; set; } = default!;

    public Guid ComissaoId { get; set; }
    public Comissao Comissao { get; set; } = default!;

    // Coordenador principal da comissão no evento (opcional)
    public Guid? CoordenadorUsuarioId { get; set; }
    public Usuario? CoordenadorUsuario { get; set; }

    // Opcional: observações, metas, etc.
    public string? Observacoes { get; set; }

    public ICollection<InscricaoTrabalhador> InscricoesTrabalhadores { get; set; } = new List<InscricaoTrabalhador>();
    public ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();
}
