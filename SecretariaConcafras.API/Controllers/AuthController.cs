using SecretariaConcafras.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using SecretariaConcafras.Application.Interfaces.Services;

namespace SecretariaConcafras.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _service;

    public AuthController(IUsuarioService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var token = await _service.LoginAsync(login.Email, login.Senha);
        return token == null ? Unauthorized("Credenciais inválidas") : Ok(new { token });
    }
}
