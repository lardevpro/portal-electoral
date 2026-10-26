namespace PortalElectoral.Models
{
    public class PartidoPolitico
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Siglas { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public ICollection<Candidatura> Candidaturas { get; set; }
        = new List<Candidatura>();
        public ICollection<PropuestaElectoral> Propuestas { get; set; }
        = new List<PropuestaElectoral>();
    }
}
