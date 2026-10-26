using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalElectoral.Data;
using PortalElectoral.DTOs;
using PortalElectoral.Models;
namespace PortalElectoral.Controllers;

[ApiController]
[Route("api/elecciones")]
public class EleccionesController : ControllerBase
{
    private readonly ElectoralDbContext _db;
    public EleccionesController(ElectoralDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EleccionDto>>>
   ObtenerTodas()
    {
        var elecciones = await _db.Elecciones
        .AsNoTracking()
        .OrderByDescending(e => e.FechaConvocatoria)
        .Select(e => new EleccionDto
        {
            Id = e.Id,
            Nombre = e.Nombre,
            FechaConvocatoria = e.FechaConvocatoria,
            FechaVotacion = e.FechaVotacion,
            Estado = e.Estado,
            EsSimulacion = e.EsSimulacion,
            NumeroCandidaturas = e.Candidaturas.Count
        })
        .ToListAsync();
        return Ok(elecciones);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EleccionDto>> ObtenerPorId(int id)
    {
        var eleccion = await _db.Elecciones
        .AsNoTracking()
        .Where(e => e.Id == id)
        .Select(e => new EleccionDto
        {
            Id = e.Id,
            Nombre = e.Nombre,
            FechaConvocatoria = e.FechaConvocatoria,
            FechaVotacion = e.FechaVotacion,
            Estado = e.Estado,
            EsSimulacion = e.EsSimulacion,
            NumeroCandidaturas = e.Candidaturas.Count
        })
        .FirstOrDefaultAsync();
        if (eleccion == null)
        {
            return NotFound();
        }
        return Ok(eleccion);
    }
    [HttpPost]
    public async Task<ActionResult<EleccionDto>> Crear(
    GuardarEleccionDto dto)
    {
        if (dto.FechaVotacion.HasValue
        && dto.FechaVotacion.Value < dto.FechaConvocatoria)
        {
            return BadRequest(
            "La fecha de votación no puede ser anterior a la convocatoria.");
        }
        var eleccion = new Eleccion
        {
            Nombre = dto.Nombre.Trim(),
            FechaConvocatoria = dto.FechaConvocatoria,
            FechaVotacion = dto.FechaVotacion,
            Estado = dto.Estado,
            EsSimulacion = dto.EsSimulacion
        };
        _db.Elecciones.Add(eleccion);
        await _db.SaveChangesAsync();
        var respuesta = new EleccionDto
        {
            Id = eleccion.Id,
            Nombre = eleccion.Nombre,
            FechaConvocatoria = eleccion.FechaConvocatoria,
            FechaVotacion = eleccion.FechaVotacion,
            Estado = eleccion.Estado,
            EsSimulacion = eleccion.EsSimulacion,
            NumeroCandidaturas = 0
        };
        return CreatedAtAction(
        nameof(ObtenerPorId),
        new { id = eleccion.Id },
        respuesta);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
    int id,
    GuardarEleccionDto dto)
    {
        if (dto.FechaVotacion.HasValue
        && dto.FechaVotacion.Value < dto.FechaConvocatoria)
        {
            return BadRequest(
            "La fecha de votación no puede ser anterior a la convocatoria.");
        }
        var eleccion = await _db.Elecciones.FindAsync(id);
        if (eleccion == null)
        {
            return NotFound();
        }
        eleccion.Nombre = dto.Nombre.Trim();
        eleccion.FechaConvocatoria = dto.FechaConvocatoria;
        eleccion.FechaVotacion = dto.FechaVotacion;
        eleccion.Estado = dto.Estado;
        eleccion.EsSimulacion = dto.EsSimulacion;
        await _db.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eleccion = await _db.Elecciones.FindAsync(id);
        if (eleccion == null)
        {
            return NotFound();
        }
        bool tieneCandidaturas = await _db.Candidaturas
        .AnyAsync(c => c.EleccionId == id);
        if (tieneCandidaturas)
        {
            return Conflict(
            "No se puede eliminar una elección que tiene  candidaturas.");
        }
        _db.Elecciones.Remove(eleccion);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}