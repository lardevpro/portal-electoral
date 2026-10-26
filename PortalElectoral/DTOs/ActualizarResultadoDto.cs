using System.ComponentModel.DataAnnotations;

namespace PortalElectoral.DTOs
{
    public class ActualizarResultadoDto
    {
        [Range(0, int.MaxValue)]
        public int Votos { get; set; }
        [Range(0, 100)]
        public int EscanosObtenidos { get; set; }
    }

}
