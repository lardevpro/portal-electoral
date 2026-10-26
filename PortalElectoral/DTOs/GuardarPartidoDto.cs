using System.ComponentModel.DataAnnotations;

namespace PortalElectoral.DTOs
{
    public class GuardarPartidoDto
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        [StringLength(15)]
        public string Siglas { get; set; } = string.Empty;
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;
    }
}
