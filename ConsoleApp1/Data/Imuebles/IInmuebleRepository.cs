using ConsoleApp1.Models;

namespace ConsoleApp1.Data.Imuebles;

public interface IInmuebleRepository
{
    Task<bool> SaveChanges();

    Task<IEnumerable<Inmueble>> GetAllInmuebles();

    Task<Inmueble> GetInmuebleById(int id);

    Task CreateInmueble(Inmueble inmueble);

    Task DeleteInmueble(int id);
    
}