using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Infrastructure;
using System;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CidadeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CidadeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{estadoId}")]
        public async Task<IActionResult> GetByEstado(Guid estadoId)
        {
            var cidades = await _context.Cidades
                .Where(c => c.EstadoId == estadoId)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            return Ok(cidades);
        }

		[HttpGet("buscar")]
		public async Task<IActionResult> BuscarPorNomeEUf(string nome, string uf)
		{
			var cidade = await _context.Cidades
				.Include(c => c.Estado)
				.FirstOrDefaultAsync(c => c.Nome == nome && c.Estado.Sigla == uf);

			if (cidade == null)
				return NotFound();

			return Ok(new
			{
				id = cidade.Id,
				nome = cidade.Nome,
				estado = new { cidade.Estado.Nome, cidade.Estado.Sigla }
			});
		}
	}
}
