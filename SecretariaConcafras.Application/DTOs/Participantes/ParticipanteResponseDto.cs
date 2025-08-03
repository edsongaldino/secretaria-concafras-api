namespace SecretariaConcafras.Application.DTOs.Participantes
{
    public class ParticipanteResponseDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Documento { get; set; }

        public string? ResponsavelNome { get; set; }
        public string? InstituicaoNome { get; set; }

        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
