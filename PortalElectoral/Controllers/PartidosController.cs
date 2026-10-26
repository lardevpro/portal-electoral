using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalElectoral.Data;
using PortalElectoral.DTOs;
using PortalElectoral.Models;
namespace PortalElectoral.Controllers;

[ApiController]
[Route("api/partidos")]
public class PartidosController : ControllerBase
{
    private readonly ElectoralDbContext _db;
    public PartidosController(ElectoralDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartidoDto>>>
   ObtenerTodos()
    {
        var partidos = await _db.Partidos
        .AsNoTracking()
        .OrderBy(p => p.Nombre)
        .Select(p => new PartidoDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Siglas = p.Siglas,
            Descripcion = p.Descripcion,
            NumeroPropuestas = p.Propuestas.Count,
            NumeroCandidaturas = p.Candidaturas.Count
        })
        .ToListAsync();
        return Ok(partidos);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PartidoDto>> ObtenerPorId(int id)
    {
        var partido = await _db.Partidos
        .AsNoTracking()
        .Where(p => p.Id == id)
        .Select(p => new PartidoDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Siglas = p.Siglas,
            Descripcion = p.Descripcion,
            NumeroPropuestas = p.Propuestas.Count,
            NumeroCandidaturas = p.Candidaturas.Count
        })
        .FirstOrDefaultAsync();
        if (partido == null)
        {
            return NotFound();
        }
        return Ok(partido);
    }
    [HttpPost]
    public async Task<ActionResult<PartidoDto>> Crear(
    GuardarPartidoDto dto)
    {
        string siglas = dto.Siglas.Trim().ToUpperInvariant();
        if (await _db.Partidos.AnyAsync(p => p.Siglas == siglas))
        {
            return Conflict("Ya existe un partido con esas siglas.");
        }
        var partido = new PartidoPolitico
        {
            Nombre = dto.Nombre.Trim(),
            Siglas = siglas,
            Descripcion = dto.Descripcion.Trim()
        };
        _db.Partidos.Add(partido);
        await _db.SaveChangesAsync();
        var respuesta = new PartidoDto
        {
            Id = partido.Id,
            Nombre = partido.Nombre,
            Siglas = partido.Siglas,
            Descripcion = partido.Descripcion,
            NumeroPropuestas = 0,
            NumeroCandidaturas = 0
        };
        return CreatedAtAction(
        nameof(ObtenerPorId),
        new { id = partido.Id },
        respuesta);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
    int id,
    GuardarPartidoDto dto)
    {
        var partido = await _db.Partidos.FindAsync(id);
        if (partido == null)
        {
            return NotFound();
        }
        string siglas = dto.Siglas.Trim().ToUpperInvariant();
        bool siglasDuplicadas = await _db.Partidos
        .AnyAsync(p => p.Id != id && p.Siglas == siglas);
        if (siglasDuplicadas)
        {
            return Conflict("Ya existe otro partido con esas siglas.");
        }
        partido.Nombre = dto.Nombre.Trim();
        partido.Siglas = siglas;
        partido.Descripcion = dto.Descripcion.Trim();
        await _db.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var partido = await _db.Partidos.FindAsync(id);
        if (partido == null)
        {
            return NotFound();
        }
        bool tieneCandidaturas = await _db.Candidaturas
        .AnyAsync(c => c.PartidoPoliticoId == id);
        bool tienePropuestas = await _db.PropuestasElectorales
        .AnyAsync(p => p.PartidoPoliticoId == id);
        if (tieneCandidaturas || tienePropuestas)
        {
            return Conflict(
            "No se puede eliminar un partido que tiene candidaturas o propuestas.");
        }
        _db.Partidos.Remove(partido);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}