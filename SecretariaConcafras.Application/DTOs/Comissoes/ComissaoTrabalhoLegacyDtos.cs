// Application/DTOs/Comissoes/ComissaoTrabalhoLegacyDtos.cs
namespace SecretariaConcafras.Application.DTOs.Comissoes
{
    public class ComissaoTrabalhoResponseDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = default!;
        public Guid EventoId { get; set; }
        public string EventoTitulo { get; set; } = default!;
        public List<UsuarioComissaoResponseDto> Usuarios { get; set; } = new();
    }

    public class UsuarioComissaoResponseDto
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; } = default!;
        public string Email { get; set; } = string.Empty;
        public string Perfil { get; set; } = default!; // "Coordenação" | "Voluntário"
    }
}
