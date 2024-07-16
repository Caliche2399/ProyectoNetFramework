using System.Net;
using AutoMapper;
using ConsoleApp1.Data.Imuebles;
using ConsoleApp1.Dtos.InmuebleDtos;
using ConsoleApp1.Middleware;
using ConsoleApp1.Models;
using Microsoft.AspNetCore.Mvc;
namespace ConsoleApp1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InmuebleController : ControllerBase
{
    private readonly IInmuebleRepository _repository;
    private IMapper _mapper;

    public InmuebleController(IInmuebleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InmuebleResponseDto>>> GetInmuebles()
    {
        var inmuebles = await _repository.GetAllInmuebles();
        return Ok(_mapper.Map<IEnumerable<InmuebleResponseDto>>(inmuebles));
    }

    [HttpGet("{id}", Name = "GetInmuebleById")]
    public async Task<ActionResult<InmuebleResponseDto>> GetInmuebleById(int id)
    {
        var inmueble = await _repository.GetInmuebleById(id);

        if (inmueble is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.NotFound,
                new { mensaje = "No se encontro el inmueble por este {id}" }
            );
        }

        return Ok(_mapper.Map<InmuebleResponseDto>(inmueble));
    }

    [HttpPost]
    public async Task<ActionResult<InmuebleResponseDto>> CreateInmueble([FromBody] InmuebleRequestDto request)
    {
        var inmuebleModel = _mapper.Map<Inmueble>(request);
        await _repository.CreateInmueble(inmuebleModel);
        await _repository.SaveChanges();

        var inmuebleResponse = _mapper.Map<InmuebleResponseDto>(inmuebleModel);

        return CreatedAtRoute(nameof(GetInmuebleById), new { inmuebleResponse.Id }, inmuebleResponse);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteInmueble(int id)
    {
        await _repository.DeleteInmueble(id);
        await _repository.SaveChanges();
        return Ok();
    }
    
}