// Application/DTOs/Comissoes/ComissaoDtos.cs
namespace SecretariaConcafras.Application.DTOs.Comissoes
{
    public class ComissaoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = default!;
        public string? Slug { get; set; }
        public bool Ativa { get; set; } = true;
    }

    public class ComissaoCreateDto
    {
        public string Nome { get; set; } = default!;
        public string? Slug { get; set; }
        public bool Ativa { get; set; } = true;
    }

    public class ComissaoUpdateDto
    {
        public string Nome { get; set; } = default!;
        public string? Slug { get; set; }
        public bool Ativa { get; set; } = true;
    }
}
