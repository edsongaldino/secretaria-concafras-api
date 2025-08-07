using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EnderecoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var endereco = await _context.Enderecos
                .Include(e => e.Cidade)
                    .ThenInclude(c => c.Estado)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (endereco == null) return NotFound();
            return Ok(endereco);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EnderecoRequest endereco)
        {

            var newEndereco = endereco.ToEntity();

            _context.Enderecos.Add(newEndereco);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newEndereco.Id }, newEndereco);
        }
    }
}
