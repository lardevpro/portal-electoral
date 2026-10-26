namespace PortalElectoral.Models
{
    public class Eleccion
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaConvocatoria { get; set; }
        public DateTime? FechaVotacion { get; set; }
        public string Estado { get; set; } = "Simulación";
        public bool EsSimulacion { get; set; } = true;
        public ICollection<Candidatura> Candidaturas { get; set; }
        = new List<Candidatura>();
    }
}
