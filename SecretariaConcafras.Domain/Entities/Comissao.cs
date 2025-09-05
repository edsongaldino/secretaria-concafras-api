public class Comissao
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = default!;
    public string? Slug { get; set; } // opcional, p/ buscas
    public bool Ativa { get; set; } = true;

    public ICollection<ComissaoEvento> ComissoesEvento { get; set; } = new List<ComissaoEvento>();
}