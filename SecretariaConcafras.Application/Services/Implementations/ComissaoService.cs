using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Application.DTOs.Comissoes;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure;

namespace SecretariaConcafras.Application.Services
{
    public class ComissaoService : IComissaoService
    {
        private readonly ApplicationDbContext _db;
        public ComissaoService(ApplicationDbContext db) => _db = db;

        public async Task<Guid> CriarAsync(ComissaoCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new ArgumentException("Nome é obrigatório.", nameof(dto.Nome));

            var entity = new Comissao
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome.Trim(),
                Slug = dto.Slug,
                Ativa = dto.Ativa
            };

            _db.Set<Comissao>().Add(entity);
            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task AtualizarAsync(Guid id, ComissaoUpdateDto dto)
        {
            var entity = await _db.Set<Comissao>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null) throw new KeyNotFoundException("Comissão não encontrada.");

            if (!string.IsNullOrWhiteSpace(dto.Nome))
                entity.Nome = dto.Nome.Trim();

            entity.Slug = dto.Slug;
            entity.Ativa = dto.Ativa;

            await _db.SaveChangesAsync();
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            var entity = await _db.Set<Comissao>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null) return false;

            // Proteção opcional: não remover se houver vínculos em comissoes_evento
            var possuiVinculos = await _db.Set<ComissaoEvento>().AnyAsync(ce => ce.ComissaoId == id);
            if (possuiVinculos)
                throw new InvalidOperationException("Não é possível remover: existem eventos vinculados a esta comissão.");

            _db.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

		public async Task<ComissaoDto?> ObterPorIdAsync(Guid id)
		{
			return await _db.Set<Comissao>()
				.AsNoTracking()
				.Where(c => c.Id == id)
				.Select(c => new ComissaoDto(c.Id, c.Nome, c.Slug, c.Ativa))
				.FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<ComissaoDto>> ObterTodosAsync()
		{
			return await _db.Set<Comissao>()
				.AsNoTracking()
				.OrderBy(c => c.Nome)
				.Select(c => new ComissaoDto(c.Id, c.Nome, c.Slug, c.Ativa))
				.ToListAsync();
		}
	}
}
