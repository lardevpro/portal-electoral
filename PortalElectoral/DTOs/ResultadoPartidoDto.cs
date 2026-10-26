public class ResultadoPartidoDto
{
    public int PartidoPoliticoId { get; set; }
    public string NombrePartido { get; set; } = string.Empty;
    public string Siglas { get; set; } = string.Empty;
    public int TotalVotos { get; set; }
    public int TotalEscanos { get; set; }
}