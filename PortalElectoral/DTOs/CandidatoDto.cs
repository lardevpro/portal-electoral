namespace PortalElectoral.DTOs
{
    public class CandidatoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public int OrdenLista { get; set; }
        public bool EsSuplente { get; set; }
    }

}
