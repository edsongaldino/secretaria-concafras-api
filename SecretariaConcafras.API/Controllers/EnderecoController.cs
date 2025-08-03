using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure.Context;
using System;

namespace SecretariaConcafras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnderecoController(AppDbContext context)
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
        public async Task<IActionResult> Create([FromBody] Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = endereco.Id }, endereco);
        }
    }
}
