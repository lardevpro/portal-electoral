using System.ComponentModel.DataAnnotations;

namespace PortalElectoral.DTOs
{
    public class CrearCandidaturaDto
    {
        [Range(1, int.MaxValue)]
        public int EleccionId { get; set; }
        [Range(1, int.MaxValue)]
        public int ProvinciaId { get; set; }
        [Range(1, int.MaxValue)]
        public int PartidoPoliticoId { get; set; }
    }
}
