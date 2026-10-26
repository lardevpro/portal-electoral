namespace PortalElectoral.DTOs
{
    public class ProvinciaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ComunidadAutonoma { get; set; } = string.Empty;
        public int EscanosCongreso { get; set; }
    }
}
