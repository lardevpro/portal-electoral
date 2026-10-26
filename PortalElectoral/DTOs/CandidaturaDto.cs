namespace PortalElectoral.DTOs
{
    public class CandidaturaDto
    {
        public int Id { get; set; }
        public int EleccionId { get; set; }
        public string NombreEleccion { get; set; } = string.Empty;
        public int ProvinciaId { get; set; }
        public string NombreProvincia { get; set; } = string.Empty;
        public int PartidoPoliticoId { get; set; }
        public string NombrePartido { get; set; } = string.Empty;
        public string SiglasPartido { get; set; } = string.Empty;
        public int Votos { get; set; }
        public int EscanosObtenidos { get; set; }
        public List<CandidatoDto> Candidatos { get; set; } = new();
    }

}
