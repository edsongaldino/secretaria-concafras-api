namespace SecretariaConcafras.Application.DTOs.Enderecos
{
    public class EnderecoCreateDto
    {
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public Guid CidadeId { get; set; }
    }

    public class EnderecoUpdateDto : EnderecoCreateDto
    {
        public Guid Id { get; set; }
    }

    public class EnderecoResponseDto
    {
        public Guid Id { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
