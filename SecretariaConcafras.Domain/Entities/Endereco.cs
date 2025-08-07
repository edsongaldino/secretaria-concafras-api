using SecretariaConcafras.Domain.Entities;

public class Endereco
{
    public Guid Id { get; set; }
    public string Logradouro { get; set; }
    public string Numero { get; set; }
    public string? Complemento { get; set; }
    public string Bairro { get; set; }

    public Guid CidadeId { get; set; }
    public Cidade Cidade { get; set; }
}

public class EnderecoRequest
{
    public string Logradouro { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = null!;
    public Guid CidadeId { get; set; }

    public Endereco ToEntity()
    {
        return new Endereco
        {
            Logradouro = this.Logradouro,
            Numero = this.Numero,
            Complemento = this.Complemento,
            Bairro = this.Bairro,
            CidadeId = this.CidadeId
        };
    }
}
