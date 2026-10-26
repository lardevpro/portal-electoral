namespace PortalElectoral.DTOs
{
    public class PartidoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Siglas { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int NumeroPropuestas { get; set; }
        public int NumeroCandidaturas { get; set; }
    }
}
