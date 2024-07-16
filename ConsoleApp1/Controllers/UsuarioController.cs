using ConsoleApp1.Data.Usuarios;
using ConsoleApp1.Dtos.UsuarioDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleApp1.Controllers;

[Route("api/usuario")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _repository;

    public UsuarioController(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UsuarioResponseDto>> Login ([FromBody] UsuarioLoginRequestDto request)
    {
        return await _repository.Login(request);
    }
    
    [AllowAnonymous]
    [HttpPost("registrar")]
    public async Task<ActionResult<UsuarioResponseDto>> registrar ([FromBody] UsuarioRegistroRequestDto request)
    {
        return await _repository.RegistroUsuario(request);
    }
    
    [HttpGet]
    public async Task<ActionResult<UsuarioResponseDto>> GetUsuario ()
    {
        return await _repository.GetUsuario();
    }
    
}