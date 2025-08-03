using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Infrastructure.Context;
using System;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstadoController(AppDbContext context)
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
