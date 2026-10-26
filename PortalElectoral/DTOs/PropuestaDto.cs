namespace PortalElectoral.DTOs
{
    public class PropuestaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int PartidoPoliticoId { get; set; }
        public string NombrePartido { get; set; } = string.Empty;
        public string SiglasPartido { get; set; } = string.Empty;
    }
}
