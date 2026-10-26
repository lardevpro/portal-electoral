using System.ComponentModel.DataAnnotations;

namespace PortalElectoral.DTOs
{
    public class GuardarEleccionDto
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaConvocatoria { get; set; }
        public DateTime? FechaVotacion { get; set; }
        [Required]
        [RegularExpression("^(Planificación|Simulación|En curso|Finalizada)$")]
        public string Estado { get; set; } = "Simulación";
        public bool EsSimulacion { get; set; } = true;
    }
}
