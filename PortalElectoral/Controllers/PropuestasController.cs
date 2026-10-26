using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalElectoral.Data;
using PortalElectoral.DTOs;
using PortalElectoral.Models;
namespace PortalElectoral.Controllers;

[ApiController]
[Route("api/propuestas")]
public class PropuestasController : ControllerBase
{
    private readonly ElectoralDbContext _db;
    public PropuestasController(ElectoralDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropuestaDto>>>
   ObtenerTodas(
    [FromQuery] int? partidoId,
    [FromQuery] string? area)
    {
        var consulta = _db.PropuestasElectorales
        .AsNoTracking()
        .AsQueryable();
        if (partidoId.HasValue)
        {
            consulta = consulta.Where(
            p => p.PartidoPoliticoId == partidoId.Value);
        }
        if (!string.IsNullOrWhiteSpace(area))
        {
            consulta = consulta.Where(
            p => p.Area == area.Trim());
        }
        var propuestas = await consulta
        .OrderBy(p => p.Area)
        .ThenBy(p => p.Titulo)
        .Select(p => new PropuestaDto
        {
            Id = p.Id,
            Titulo = p.Titulo,
            Area = p.Area,
            Descripcion = p.Descripcion,
            PartidoPoliticoId = p.PartidoPoliticoId,
            NombrePartido = p.PartidoPolitico.Nombre,
            SiglasPartido = p.PartidoPolitico.Siglas
        }).ToListAsync();
        return Ok(propuestas);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PropuestaDto>> ObtenerPorId(int id)
    {
        var propuesta = await _db.PropuestasElectorales
        .AsNoTracking().Where(p => p.Id == id).Select(p => new PropuestaDto
        {
            Id = p.Id,
            Titulo = p.Titulo,
            Area = p.Area,
            Descripcion = p.Descripcion,
            PartidoPoliticoId = p.PartidoPoliticoId,
            NombrePartido = p.PartidoPolitico.Nombre,
            SiglasPartido = p.PartidoPolitico.Siglas
        })
        .FirstOrDefaultAsync();
        if (propuesta == null)
        {
            return NotFound();
        }
        return Ok(propuesta);
    }
    [HttpPost]
    public async Task<ActionResult<PropuestaDto>> Crear(
    GuardarPropuestaDto dto)
    {
        bool existePartido = await _db.Partidos
        .AnyAsync(p => p.Id == dto.PartidoPoliticoId);
        if (!existePartido)
        {
            return BadRequest("El partido indicado no existe.");
        }
        var propuesta = new PropuestaElectoral
        {
            Titulo = dto.Titulo.Trim(),
            Area = dto.Area.Trim(),
            Descripcion = dto.Descripcion.Trim(),
            PartidoPoliticoId = dto.PartidoPoliticoId
        };
        _db.PropuestasElectorales.Add(propuesta);
        await _db.SaveChangesAsync();
        var respuesta = await _db.PropuestasElectorales
        .AsNoTracking()
        .Where(p => p.Id == propuesta.Id)
        .Select(p => new PropuestaDto
        {
            Id = p.Id,
            Titulo = p.Titulo,
            Area = p.Area,
            Descripcion = p.Descripcion,
            PartidoPoliticoId = p.PartidoPoliticoId,
            NombrePartido = p.PartidoPolitico.Nombre,
            SiglasPartido = p.PartidoPolitico.Siglas
        })
        .FirstAsync();
        return CreatedAtAction(
        nameof(ObtenerPorId),
        new { id = propuesta.Id },
        respuesta);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
    int id,
    GuardarPropuestaDto dto)
    {
        var propuesta = await _db.PropuestasElectorales.FindAsync(id);
        if (propuesta == null)
        {
            return NotFound();
        }
        bool existePartido = await _db.Partidos
        .AnyAsync(p => p.Id == dto.PartidoPoliticoId);
        if (!existePartido)
        {
            return BadRequest("El partido indicado no existe.");
        }
        propuesta.Titulo = dto.Titulo.Trim();
        propuesta.Area = dto.Area.Trim();
        propuesta.Descripcion = dto.Descripcion.Trim();
        propuesta.PartidoPoliticoId = dto.PartidoPoliticoId;
        await _db.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var propuesta = await _db.PropuestasElectorales.FindAsync(id);
        if (propuesta == null)
        {
            return NotFound();
        }
        _db.PropuestasElectorales.Remove(propuesta);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}