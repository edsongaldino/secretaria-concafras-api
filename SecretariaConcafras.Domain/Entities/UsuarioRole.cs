using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Domain.Enums;

public class UsuarioRole
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = default!;

    public RoleSistema Role { get; set; }

    public Guid? EventoId { get; set; }
    public Evento? Evento { get; set; }

    public Guid? ComissaoEventoId { get; set; }
    public ComissaoEvento? ComissaoEvento { get; set; }
}