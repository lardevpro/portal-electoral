using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalElectoral.Data;
using PortalElectoral.DTOs;
namespace PortalElectoral.Controllers;

[ApiController]
[Route("api/estadisticas")]
public class EstadisticasController : ControllerBase
{
    private readonly ElectoralDbContext _db;
    public EstadisticasController(ElectoralDbContext db)
    {
        _db = db;
    }
    [HttpGet("elecciones/{eleccionId:int}/resultados")]
    public async Task<ActionResult<IEnumerable<ResultadoPartidoDto>>>
    ObtenerResultados(int eleccionId)
    {
        bool existeEleccion = await _db.Elecciones
        .AnyAsync(e => e.Id == eleccionId);
        if (!existeEleccion)
        {
            return NotFound("La elección indicada no existe.");
        }
        var resultados = await _db.Candidaturas
        .AsNoTracking()
        .Where(c => c.EleccionId == eleccionId)
        .GroupBy(c => new
        {
            c.PartidoPoliticoId,
            c.PartidoPolitico.Nombre,
            c.PartidoPolitico.Siglas
        })
        .Select(grupo => new ResultadoPartidoDto
        {
            PartidoPoliticoId = grupo.Key.PartidoPoliticoId,
            NombrePartido = grupo.Key.Nombre,
            Siglas = grupo.Key.Siglas,
            TotalVotos = grupo.Sum(c => c.Votos),
            TotalEscanos = grupo.Sum(c => c.EscanosObtenidos)
        })
        .OrderByDescending(r => r.TotalVotos)
        .ToListAsync();
        return Ok(resultados);
    }

    [HttpGet("elecciones/{eleccionId:int}/provincias/{provinciaId:int}")]
    public async Task<ActionResult<IEnumerable<ResultadoProvinciaDto>>>
     ObtenerResultadosProvincia(
     int eleccionId,
     int provinciaId)
    {
        bool existeProvincia = await _db.Provincias
        .AnyAsync(p => p.Id == provinciaId);
        if (!existeProvincia)
        {
            return NotFound("La provincia indicada no existe.");
        }
        var resultados = await _db.Candidaturas
        .AsNoTracking()
        .Where(c =>
        c.EleccionId == eleccionId
        && c.ProvinciaId == provinciaId)
        .OrderByDescending(c => c.Votos)
        .Select(c => new ResultadoProvinciaDto
        {
            CandidaturaId = c.Id,
            NombrePartido = c.PartidoPolitico.Nombre,
            Siglas = c.PartidoPolitico.Siglas,
            Votos = c.Votos,
            EscanosObtenidos = c.EscanosObtenidos
        })
        .ToListAsync();
        return Ok(resultados);
    }
}