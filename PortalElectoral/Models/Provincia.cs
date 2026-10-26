namespace PortalElectoral.Models
{
    public class Provincia
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ComunidadAutonoma { get; set; } = string.Empty;
        public int EscanosCongreso { get; set; }
        public ICollection<Candidatura> Candidaturas { get; set; }
        = new List<Candidatura>();
    }

}
