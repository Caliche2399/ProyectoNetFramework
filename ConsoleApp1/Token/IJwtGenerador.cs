using ConsoleApp1.Models;

namespace ConsoleApp1.Token;

public interface IJwtGenerador
{
    string CrearToken(Usuario usuario);
}