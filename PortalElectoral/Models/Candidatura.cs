namespace PortalElectoral.Models
{
    public class Candidatura
    {
        public int Id { get; set; }
        public int EleccionId { get; set; }
        public int ProvinciaId { get; set; }
        public int PartidoPoliticoId { get; set; }
        public int Votos { get; set; }
        public int EscanosObtenidos { get; set; }
        public Eleccion Eleccion { get; set; } = null!;
        public Provincia Provincia { get; set; } = null!;
        public PartidoPolitico PartidoPolitico { get; set; } = null!;
        public ICollection<Candidato> Candidatos { get; set; }
        = new List<Candidato>();
    }
}
