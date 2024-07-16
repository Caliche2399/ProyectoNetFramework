using AutoMapper;
using ConsoleApp1.Dtos.InmuebleDtos;
using ConsoleApp1.Models;

namespace ConsoleApp1.Profiles;

public class InmuebleProfile : Profile
{
    public InmuebleProfile()
    {
        CreateMap<Inmueble, InmuebleResponseDto>();
        CreateMap<InmuebleRequestDto, Inmueble>();
    }
}