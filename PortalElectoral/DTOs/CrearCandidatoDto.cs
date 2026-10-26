using System.ComponentModel.DataAnnotations;

public class CrearCandidatoDto
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    [StringLength(150)]
    public string Apellidos { get; set; } = string.Empty;
    [Range(1, 500)]
    public int OrdenLista { get; set; }
    public bool EsSuplente { get; set; }
}