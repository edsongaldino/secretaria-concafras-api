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
    }
}
