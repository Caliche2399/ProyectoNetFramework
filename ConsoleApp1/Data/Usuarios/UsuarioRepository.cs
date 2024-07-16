using System.Net;
using ConsoleApp1.Dtos.UsuarioDtos;
using ConsoleApp1.Middleware;
using ConsoleApp1.Models;
using ConsoleApp1.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Data.Usuarios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IJwtGenerador _jwtGenerador;
    private readonly AppDbContext _context;
    private readonly IUsuarioSesion _usuarioSesion;

    
    public UsuarioRepository(UserManager<Usuario> userManager, 
        SignInManager<Usuario> signInManager, 
        IJwtGenerador jwtGenerador, 
        AppDbContext context, 
        IUsuarioSesion usuarioSesion)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtGenerador = jwtGenerador;
        _context = context;
        _usuarioSesion = usuarioSesion;
    }

    private UsuarioResponseDto TransformerUserToUserDto(Usuario usuario)
    {
        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Telefono = usuario.Telefono,
            Email = usuario.Email,
            UserName = usuario.UserName,
            Token = _jwtGenerador.CrearToken(usuario)
        };
    }
    
    public async Task<UsuarioResponseDto> GetUsuario()
    {
        var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
        
        if (usuario is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.Unauthorized, 
                new {mensaje= "El usuario no existe en la bd"}
            );
        }
        
        return  TransformerUserToUserDto(usuario!);
    }

    public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto request)
    {
        var usuario = await _userManager.FindByEmailAsync(request.Email!);
        
        if (usuario is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.Unauthorized, 
                new {mensaje= "El email del usuario no existe en la BD"}
            );
        }

        var resultado = await _signInManager.CheckPasswordSignInAsync(usuario!, request.Password!, false);

        if (resultado.Succeeded)
        {
            return TransformerUserToUserDto(usuario);
        }

        throw new MiddlewareException(
            HttpStatusCode.Unauthorized,
            new {mensaje = "Las credenciales son incorrectas"}
        );
    }

    public async Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRequestDto request)
    {
        var existeEmail = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
        
        if (existeEmail)
        {
            throw new MiddlewareException(
                HttpStatusCode.BadRequest, 
                new { mensaje = "El email ya está registrado" }
            );
        }
        
        var existeUsername = await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync();
        
        if (existeUsername)
        {
            throw new MiddlewareException(
                HttpStatusCode.BadRequest, 
                new { mensaje = "El UserName ya está registrado" }
            );
        }
        
        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Telefono = request.Telefono,
            Email = request.Email,
            UserName = request.UserName
        };

        var resultado = await _userManager.CreateAsync(usuario, request.Password!);

        if (resultado.Succeeded)
        {
            return TransformerUserToUserDto(usuario);
        }

        throw new Exception("No se pudo registrar el usuario");
    }
}