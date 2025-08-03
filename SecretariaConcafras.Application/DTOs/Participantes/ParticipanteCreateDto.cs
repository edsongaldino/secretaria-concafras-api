namespace SecretariaConcafras.Application.DTOs.Participantes
{
    public class ParticipanteCreateDto
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Documento { get; set; } // CPF, RG etc.

        public Guid? ResponsavelId { get; set; }
        public Guid? InstituicaoId { get; set; }
        public Guid EnderecoId { get; set; }
    }
}
