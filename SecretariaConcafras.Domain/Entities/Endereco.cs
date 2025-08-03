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
