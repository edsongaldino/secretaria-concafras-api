using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Infrastructure;
using System;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstadoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var estados = await _context.Estados
                .OrderBy(e => e.Nome)
                .ToListAsync();

            return Ok(estados);
        }
    }
}
