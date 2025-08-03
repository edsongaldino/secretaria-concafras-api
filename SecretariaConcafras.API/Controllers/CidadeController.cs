using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Infrastructure.Context;
using System;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CidadeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CidadeController(AppDbContext context)
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
