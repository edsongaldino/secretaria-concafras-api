using SecretariaConcafras.Domain.Entities;

public class Endereco
{
    public Guid Id { get; set; }
    public string Logradouro { get; set; }
    public string Numero { get; set; }
    public string? Complemento { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
}

public class EnderecoRequest
{
    public string Logradouro { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = null!;
    public string Cidade { get; set; }
    public string Estado { get; set; }

    public Endereco ToEntity()
    {
        return new Endereco
        {
            Id = new Guid(),
            Logradouro = this.Logradouro,
            Numero = this.Numero,
            Complemento = this.Complemento,
            Bairro = this.Bairro,
            Cidade = this.Cidade,
            Estado = this.Estado
        };
    }
}
