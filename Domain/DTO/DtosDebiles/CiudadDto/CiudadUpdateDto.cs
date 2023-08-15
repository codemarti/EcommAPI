namespace Domain.DTO.DtosDebiles.CiudadDto
{
    public class CiudadUpdateDto
    {
        [Required]
        public int IdCiudad { get; set; }
        [Required]
        public string? NombreCiudad { get; set; }

        // Llave foranea
        [Required]
        public int EstadoId { get; set; }
    }
}
