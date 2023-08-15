namespace Domain.DTO.DtosDebiles.CiudadDto
{
    public class CiudadCreateDto
    {
        [Required]
        public string? NombreCiudad { get; set; }
        // Llave foranea
        [Required]
        public int EstadoId { get; set; }
    }
}
