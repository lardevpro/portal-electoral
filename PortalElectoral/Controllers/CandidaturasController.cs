using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalElectoral.Data;
using PortalElectoral.DTOs;
using PortalElectoral.Models;
namespace PortalElectoral.Controllers;

[ApiController]
[Route("api/candidaturas")]
public class CandidaturasController : ControllerBase
{
    private readonly ElectoralDbContext _db;
    public CandidaturasController(ElectoralDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CandidaturaDto>>>
   ObtenerTodas(
    [FromQuery] int? eleccionId,
    [FromQuery] int? provinciaId,
    [FromQuery] int? partidoId)
    {
        var consulta = _db.Candidaturas
        .AsNoTracking()
        .AsQueryable();
        if (eleccionId.HasValue)
        {
            consulta = consulta.Where(
            c => c.EleccionId == eleccionId.Value);
        }
        if (provinciaId.HasValue)
        {
            consulta = consulta.Where(
            c => c.ProvinciaId == provinciaId.Value);
        }
        if (partidoId.HasValue)
        {
            consulta = consulta.Where(
            c => c.PartidoPoliticoId == partidoId.Value);
        }
        var candidaturas = await consulta
        .Include(c => c.Eleccion)
        .Include(c => c.Provincia)
        .Include(c => c.PartidoPolitico)
        .Include(c => c.Candidatos)
        .OrderBy(c => c.Provincia.Nombre)
        .ThenBy(c => c.PartidoPolitico.Nombre)
        .ToListAsync();
        return Ok(candidaturas.Select(ConvertirDto));
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CandidaturaDto>> ObtenerPorId(int id)
    {
        var candidatura = await _db.Candidaturas
        .AsNoTracking()
        .Include(c => c.Eleccion)
        .Include(c => c.Provincia)
        .Include(c => c.PartidoPolitico)
        .Include(c => c.Candidatos)
        .FirstOrDefaultAsync(c => c.Id == id);
        if (candidatura == null)
        {
            return NotFound();
        }
        return Ok(ConvertirDto(candidatura));
    }
    [HttpPost]
    public async Task<ActionResult<CandidaturaDto>> Crear(CrearCandidaturaDto dto)
    {
        bool existeEleccion = await _db.Elecciones
        .AnyAsync(e => e.Id == dto.EleccionId);
        bool existeProvincia = await _db.Provincias
        .AnyAsync(p => p.Id == dto.ProvinciaId);
        bool existePartido = await _db.Partidos
        .AnyAsync(p => p.Id == dto.PartidoPoliticoId);
        if (!existeEleccion || !existeProvincia || !existePartido)
        {
            return BadRequest(
            "La elección, provincia o partido indicado no existe.");
        }
        bool yaExiste = await _db.Candidaturas.AnyAsync(c =>
        c.EleccionId == dto.EleccionId
        && c.ProvinciaId == dto.ProvinciaId
        && c.PartidoPoliticoId == dto.PartidoPoliticoId);
        if (yaExiste)
        {
            return Conflict(
            "Ya existe una candidatura para esa elección, provincia  y partido.");
        }
        var candidatura = new Candidatura
        {
            EleccionId = dto.EleccionId,
            ProvinciaId = dto.ProvinciaId,
            PartidoPoliticoId = dto.PartidoPoliticoId,
            Votos = 0,
            EscanosObtenidos = 0
        };
        _db.Candidaturas.Add(candidatura);
        await _db.SaveChangesAsync();
        var nueva = await _db.Candidaturas
        .Include(c => c.Eleccion)
        .Include(c => c.Provincia)
        .Include(c => c.PartidoPolitico)
        .Include(c => c.Candidatos)
        .FirstAsync(c => c.Id == candidatura.Id);
        return CreatedAtAction(
        nameof(ObtenerPorId),
        new { id = candidatura.Id },
        ConvertirDto(nueva));
    }
    [HttpPut("{id:int}/resultado")]
    public async Task<IActionResult> ActualizarResultado(
    int id,
    ActualizarResultadoDto dto)
    {
        var candidatura = await _db.Candidaturas.FindAsync(id);
        if (candidatura == null)
        {
            return NotFound();
        }
        var provincia = await _db.Provincias
        .AsNoTracking()
        .FirstAsync(p => p.Id == candidatura.ProvinciaId);
        if (dto.EscanosObtenidos > provincia.EscanosCongreso)
        {
            return BadRequest(
            "Los escaños asignados no pueden superar el total de la provincia.");
        }
        candidatura.Votos = dto.Votos;
        candidatura.EscanosObtenidos = dto.EscanosObtenidos;
        await _db.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var candidatura = await _db.Candidaturas.FindAsync(id);
        if (candidatura == null)
        {
            return NotFound();
        }
        _db.Candidaturas.Remove(candidatura);
        await _db.SaveChangesAsync();
        return NoContent();
    }
    [HttpPost("{id:int}/candidatos")]
    public async Task<ActionResult<CandidatoDto>> CrearCandidato(int id,CrearCandidatoDto dto)
    {
        bool existeCandidatura = await _db.Candidaturas
        .AnyAsync(c => c.Id == id);
        if (!existeCandidatura)
        {
            return NotFound("La candidatura indicada no existe.");
        }
        bool ordenOcupado = await _db.Candidatos.AnyAsync(c =>
        c.CandidaturaId == id
        && c.OrdenLista == dto.OrdenLista);
        if (ordenOcupado)
        {
            return Conflict(
            "Ya existe un candidato con ese orden en la lista.");
        }
        var candidato = new Candidato
        {
            Nombre = dto.Nombre.Trim(),
            Apellidos = dto.Apellidos.Trim(),
            OrdenLista = dto.OrdenLista,
            EsSuplente = dto.EsSuplente,
            CandidaturaId = id
        };
        _db.Candidatos.Add(candidato);
        await _db.SaveChangesAsync();
        var respuesta = ConvertirDto(candidato);
        return CreatedAtAction(
        nameof(ObtenerCandidato),
        new { id, candidatoId = candidato.Id },
        respuesta);
    }
    [HttpGet("{id:int}/candidatos/{candidatoId:int}")]
    public async Task<ActionResult<CandidatoDto>> ObtenerCandidato(
    int id,
    int candidatoId)
    {
        var candidato = await _db.Candidatos
        .AsNoTracking()
        .FirstOrDefaultAsync(c =>
        c.Id == candidatoId
        && c.CandidaturaId == id);
        if (candidato == null)
        {
            return NotFound();
        }
        return Ok(ConvertirDto(candidato));
    }
    [HttpDelete("{id:int}/candidatos/{candidatoId:int}")]
    public async Task<IActionResult> EliminarCandidato(
    int id,
    int candidatoId)
    {
        var candidato = await _db.Candidatos
        .FirstOrDefaultAsync(c =>
        c.Id == candidatoId
        && c.CandidaturaId == id);
        if (candidato == null)
        {
            return NotFound();
        }
        _db.Candidatos.Remove(candidato);
        await _db.SaveChangesAsync();
        return NoContent();
    }
    private static CandidaturaDto ConvertirDto(Candidatura c)
    {
        return new CandidaturaDto
        {
            Id = c.Id,
            EleccionId = c.EleccionId,
            NombreEleccion = c.Eleccion.Nombre,
            ProvinciaId = c.ProvinciaId,
            NombreProvincia = c.Provincia.Nombre,
            PartidoPoliticoId = c.PartidoPoliticoId,
            NombrePartido = c.PartidoPolitico.Nombre,
            SiglasPartido = c.PartidoPolitico.Siglas,
            Votos = c.Votos,
            EscanosObtenidos = c.EscanosObtenidos,
            Candidatos = c.Candidatos
        .OrderBy(ca => ca.OrdenLista)
        .Select(ConvertirDto)
        .ToList()
        };
    }
    private static CandidatoDto ConvertirDto(Candidato c)
    {
        return new CandidatoDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Apellidos = c.Apellidos,
            OrdenLista = c.OrdenLista,
            EsSuplente = c.EsSuplente
        };
    }
}