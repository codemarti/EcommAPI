namespace Domain.DTO.DtosDebiles.EstadoDto
{
    public class EstadoGetDto
    {
        [Required]
        public int IdEstado { get; set; }
        [Required]
        public string? NombreEstado { get; set; }
        [Required]
        public string? CodigoEstado { get; set; }

        // Llave foranea
        [Required]
        public Pais? Pais { get; set; }
    }
}
