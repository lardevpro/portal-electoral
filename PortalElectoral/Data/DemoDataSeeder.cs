using Microsoft.EntityFrameworkCore;
using PortalElectoral.Models;
namespace PortalElectoral.Data;

public static class DemoDataSeeder
{
    public static async Task InicializarAsync(
    ElectoralDbContext db)
    {
        if (await db.Elecciones.AnyAsync())
        {
            return;
        }
        var eleccion = new Eleccion
        {
            Nombre = "Simulacro General de Aula 2026",
            FechaConvocatoria = new DateTime(2026, 10, 18),
            FechaVotacion = null,
            Estado = "Simulación",
            EsSimulacion = true
        };
        var madrid = new Provincia
        {
            Nombre = "Madrid",
            ComunidadAutonoma = "Comunidad de Madrid",
            EscanosCongreso = 10
        };
        var toledo = new Provincia
        {
            Nombre = "Toledo",
            ComunidadAutonoma = "Castilla-La Mancha",
            EscanosCongreso = 5
        };
        var horizonte = new PartidoPolitico
        {
            Nombre = "Horizonte Cívico",
            Siglas = "HC",
            Descripcion =
        "Partido ficticio utilizado para las pruebas de la API."
        };
        var barrios = new PartidoPolitico
        {
            Nombre = "Pacto de Barrios",
            Siglas = "PB",
            Descripcion =
        "Partido ficticio utilizado para las pruebas de la API."
        };
        var renovacion = new PartidoPolitico
        {
            Nombre = "Renovación Común",
            Siglas = "RC",
            Descripcion =
        "Partido ficticio utilizado para las pruebas de la API."
        };
        db.Elecciones.Add(eleccion);
        db.Provincias.AddRange(madrid, toledo);
        db.Partidos.AddRange(
        horizonte,
        barrios,
        renovacion);
        await db.SaveChangesAsync();
        db.PropuestasElectorales.AddRange(
        new PropuestaElectoral
        {
            Titulo = "Mejora de la conectividad rural",
            Area = "Infraestructuras",
            Descripcion =
        "Propuesta ficticia para mejorar la conectividad digital.",
       
        PartidoPoliticoId = horizonte.Id
        },
        new PropuestaElectoral
        {
            Titulo = "Refuerzo de la atención primaria",
            Area = "Sanidad",
            Descripcion =
        "Propuesta ficticia para reducir los tiempos de espera.",
       
        PartidoPoliticoId = barrios.Id
        },
        new PropuestaElectoral
        {
            Titulo = "Formación tecnológica",
            Area = "Educación",
            Descripcion =
        "Propuesta ficticia para ampliar la formación digital.",
       
        PartidoPoliticoId = renovacion.Id
        },
        new PropuestaElectoral
        {
            Titulo = "Transporte público local",
            Area = "Movilidad",
            Descripcion =
        "Propuesta ficticia para mejorar las conexiones locales.",
       
        PartidoPoliticoId = horizonte.Id
        },
        new PropuestaElectoral
        {
            Titulo = "Vivienda asequible",
            Area = "Vivienda",
            Descripcion =
        "Propuesta ficticia para facilitar el acceso a la vivienda.",
       
        PartidoPoliticoId = barrios.Id
        },
        new PropuestaElectoral
        {
            Titulo = "Administración digital",
            Area = "Administración pública",
            Descripcion =
        "Propuesta ficticia para simplificar los trámites digitales.",
       
        PartidoPoliticoId = renovacion.Id
        });
        db.Candidaturas.AddRange(
        new Candidatura
        {
            EleccionId = eleccion.Id,
            ProvinciaId = madrid.Id,
            PartidoPoliticoId = horizonte.Id,
            Votos = 123400,
            EscanosObtenidos = 5
        },
        new Candidatura
        {
            EleccionId = eleccion.Id,
            ProvinciaId = madrid.Id,
            PartidoPoliticoId = barrios.Id,
            Votos = 98200,
            EscanosObtenidos = 3
        },
        new Candidatura
        {
            EleccionId = eleccion.Id,
            ProvinciaId = madrid.Id,
            PartidoPoliticoId = renovacion.Id,
            Votos = 60100,
            EscanosObtenidos = 2
        },
        new Candidatura
        {
            EleccionId = eleccion.Id,
            ProvinciaId = toledo.Id,
            PartidoPoliticoId = horizonte.Id,
            Votos = 31000,
            EscanosObtenidos = 2
        },
        new Candidatura
        {
            EleccionId = eleccion.Id,
            ProvinciaId = toledo.Id,
            PartidoPoliticoId = barrios.Id,
            Votos = 28500,
            EscanosObtenidos = 2
        },
        new Candidatura
        {
            EleccionId = eleccion.Id,
            ProvinciaId = toledo.Id,
            PartidoPoliticoId = renovacion.Id,
            Votos = 19000,
            EscanosObtenidos = 1
        });
        await db.SaveChangesAsync();
        var candidaturaMadrid = await db.Candidaturas
        .SingleAsync(c =>
        c.EleccionId == eleccion.Id
        && c.ProvinciaId == madrid.Id
        && c.PartidoPoliticoId == horizonte.Id);
        db.Candidatos.AddRange(
        new Candidato
        {
            Nombre = "Nora",
            Apellidos = "Valdés",
            OrdenLista = 1,
            EsSuplente = false,
            CandidaturaId = candidaturaMadrid.Id
        },
        new Candidato
        {
            Nombre = "Mateo",
            Apellidos = "Serrano",
            OrdenLista = 2,
            EsSuplente = false,
            CandidaturaId = candidaturaMadrid.Id
        },
        new Candidato
        {
            Nombre = "Lucía",
            Apellidos = "Prado",
            OrdenLista = 3,
            EsSuplente = true,
            CandidaturaId = candidaturaMadrid.Id
        });
        await db.SaveChangesAsync();
    }
}