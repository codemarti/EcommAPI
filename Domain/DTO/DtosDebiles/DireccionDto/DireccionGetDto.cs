namespace Domain.DTO.DtosDebiles.DireccionDto
{
    public class DireccionGetDto
    {
        [Required]
        public int IdDireccion { get; set; }
        [Required]
        public string? CP { get; set; }
        [Required]
        public string? Calle1 { get; set; }
        public string? Calle2 { get; set; }
        [Required]
        public string? NumExt { get; set; }
        public string? Detalles { get; set; }

        // Llave foranea
        [Required]
        public Ciudad? Ciudad { get; set; }
    }
}
