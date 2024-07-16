using System.Net;
using ConsoleApp1.Middleware;
using ConsoleApp1.Models;
using ConsoleApp1.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Data.Imuebles;

public class InmuebleRepository : IInmuebleRepository
{
    private readonly AppDbContext _context;
    private readonly IUsuarioSesion _usuarioSesion;
    private readonly UserManager<Usuario> _userManager;
    
    //Constructor
    public InmuebleRepository(AppDbContext context, IUsuarioSesion sesion, UserManager<Usuario> userManager)
    {
        _context = context;
        _usuarioSesion = sesion;
        _userManager = userManager;
    }
    
    public async Task CreateInmueble(Inmueble inmueble)
    {
        var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

        if (usuario is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.Unauthorized, 
                new { mensaje = "El usuario no es valido" }
            );
        }

        if (inmueble is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.BadRequest, 
                new { mensaje = "Los datos del inmueble son incorrectos" }
            );
        }
        
        inmueble.FechaCreacion = DateTime.Now;
        inmueble.UsuarioId = Guid.Parse(usuario!.Id);

        await _context.Inmuebles!.AddAsync(inmueble);
    }

    public async Task<bool> SaveChanges()
    {
        return ((await _context.SaveChangesAsync() >= 0));
    }

    public async Task<IEnumerable<Inmueble>> GetAllInmuebles()
    {
        return await _context.Inmuebles!.ToListAsync();
    }

    public async Task<Inmueble> GetInmuebleById(int id)
    {
        return await _context.Inmuebles!.FirstOrDefaultAsync(p => p.Id == id)!;
    }
    

    public async Task DeleteInmueble(int id)
    {
        var inmueble = await _context.Inmuebles!.FirstOrDefaultAsync(p => p.Id == id);
        if (inmueble != null)
        {
            _context.Inmuebles!.Remove(inmueble);
        }
    }
}