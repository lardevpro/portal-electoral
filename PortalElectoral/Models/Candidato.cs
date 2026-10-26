namespace PortalElectoral.Models
{
    public class Candidato
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public int OrdenLista { get; set; }
        public bool EsSuplente { get; set; }
        public int CandidaturaId { get; set; }
        public Candidatura Candidatura { get; set; } = null!;
    }
}
