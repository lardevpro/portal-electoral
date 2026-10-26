namespace PortalElectoral.Models
{
    public class PropuestaElectoral
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int PartidoPoliticoId { get; set; }
        public PartidoPolitico PartidoPolitico { get; set; } = null!;
    }
}
