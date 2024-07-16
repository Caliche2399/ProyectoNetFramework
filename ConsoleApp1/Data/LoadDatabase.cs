using ConsoleApp1.Models;
using Microsoft.AspNetCore.Identity;

namespace ConsoleApp1.Data;

public class LoadDatabase
{
    public static async Task InsertarData(AppDbContext context, UserManager<Usuario> usuarioManager)
    {
        if (!usuarioManager.Users.Any())
        {
            var usuario = new Usuario
            {
                Nombre = "Carlos",
                Apellido = "Aguilar",
                Email = "carlosaguilarpaniagua@gmail.com",
                UserName = "carlos.aguilar",
                Telefono = "30318788"
            };

           await usuarioManager.CreateAsync(usuario, "PasswordCalichinPaniagua123$");
        }

        if (!context.Inmuebles!.Any())
        {
            context.Inmuebles.AddRange(
                new Inmueble
                {
                    Nombre = "Casa",
                    Direccion = "Ciudad",
                    Precio = 4500M,
                    FechaCreacion = DateTime.Now
                },
                new Inmueble
                {
                    Nombre = "Casa Roca",
                    Direccion = "Ciudad",
                    Precio = 3500M,
                    FechaCreacion = DateTime.Now
                }
            );
        }

        context.SaveChanges();
    }
}