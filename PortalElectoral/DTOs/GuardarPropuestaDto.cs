using System.ComponentModel.DataAnnotations;

namespace PortalElectoral.DTOs
{
    public class GuardarPropuestaDto
    {
        [Required]
        [StringLength(150)]
        public string Titulo { get; set; } = string.Empty;
        [Required]
        [StringLength(80)]
        public string Area { get; set; } = string.Empty;
        [Required]
        [StringLength(1000)]
        public string Descripcion { get; set; } = string.Empty;
        [Range(1, int.MaxValue)]
        public int PartidoPoliticoId { get; set; }
    }
}
