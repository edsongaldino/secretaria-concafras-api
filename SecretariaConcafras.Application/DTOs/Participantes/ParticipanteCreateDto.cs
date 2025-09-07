using SecretariaConcafras.Application.DTOs.Enderecos;

namespace SecretariaConcafras.Application.DTOs.Participantes
{
    public class ParticipanteCreateDto
    {
        public string Nome { get; set; } = default!;
        public DateTime? DataNascimento { get; set; }
        public string Cpf { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Telefone { get; set; }
        public string? Instituicao { get; set; }

        public EnderecoCreateDto? Endereco { get; set; }
    }

    public record ParticipanteResultadoDto(Guid Id, string Nome);
}
