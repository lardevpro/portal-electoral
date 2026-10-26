namespace PortalElectoral.DTOs
{
    public class EleccionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaConvocatoria { get; set; }
        public DateTime? FechaVotacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public bool EsSimulacion { get; set; }
        public int NumeroCandidaturas { get; set; }
    }
}
