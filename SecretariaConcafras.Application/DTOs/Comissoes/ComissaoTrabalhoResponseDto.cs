namespace SecretariaConcafras.Application.DTOs.Comissoes
{
    public class ComissaoTrabalhoResponseDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public Guid EventoId { get; set; }
        public string EventoTitulo { get; set; }

        public List<UsuarioComissaoResponseDto> Usuarios { get; set; } = new();
    }

    public class UsuarioComissaoResponseDto
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; } // "Coordenação" ou "Voluntário"
    }
}
