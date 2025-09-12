namespace SecretariaConcafras.Application.DTOs.Participantes
{
    public class ParticipanteResponseDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public DateOnly? DataNascimento { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone { get; set; }

        public string? ResponsavelNome { get; set; }
        public string? InstituicaoNome { get; set; }

        public EnderecoDto? Endereco { get; set; }

        public class EnderecoDto
        {
            public string? Cep { get; set; }
            public string? Logradouro { get; set; }
            public string? Numero { get; set; }
            public string? Complemento { get; set; }
            public string? Bairro { get; set; }
            public string? Cidade { get; set; }
            public string? Estado { get; set; }
        }
    }

}
