// Application/DTOs/Comissoes/ComissaoDtos.cs
namespace SecretariaConcafras.Application.DTOs.Comissoes
{
    public class ComissaoDto
    {
		public ComissaoDto(Guid id, string nome, string? slug, bool ativa)
		{
			Id = id;
			Nome = nome;
			Slug = slug;
			Ativa = ativa;
		}

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

	public record CriarComissaoRequest(string Nome, string? Slug, bool Ativa = true);
	public record AtualizarComissaoRequest(string Nome, string? Slug, bool Ativa = true);
}
