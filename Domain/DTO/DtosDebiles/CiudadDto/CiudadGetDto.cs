namespace Domain.DTO.DtosDebiles.CiudadDto
{
    public class CiudadGetDto
    {
        [Required]
        public int IdCiudad { get; set; }
        [Required]
        public string? NombreCiudad { get; set; }

        // Llave foranea
        [Required]
        public Estado? Estado { get; set; }
    }
}
