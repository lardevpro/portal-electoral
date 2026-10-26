using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalElectoral.Data;
using PortalElectoral.DTOs;
namespace PortalElectoral.Controllers;

[ApiController]
[Route("api/provincias")]
public class ProvinciasController : ControllerBase
{
    private readonly ElectoralDbContext _db;
    public ProvinciasController(ElectoralDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProvinciaDto>>>
   ObtenerTodas()
    {
        var provincias = await _db.Provincias
        .AsNoTracking()
        .OrderBy(p => p.Nombre)
        .Select(p => new ProvinciaDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            ComunidadAutonoma = p.ComunidadAutonoma,
            EscanosCongreso = p.EscanosCongreso
        })
        .ToListAsync();
        return Ok(provincias);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProvinciaDto>> ObtenerPorId(int id)
    {
        var provincia = await _db.Provincias
        .AsNoTracking()
        .Where(p => p.Id == id)
        .Select(p => new ProvinciaDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            ComunidadAutonoma = p.ComunidadAutonoma,
            EscanosCongreso = p.EscanosCongreso
        })
        .FirstOrDefaultAsync();
        if (provincia == null)
        {
            return NotFound();
        }
        return Ok(provincia);
    }
}
