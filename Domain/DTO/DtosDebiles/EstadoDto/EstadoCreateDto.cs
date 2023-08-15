namespace Domain.DTO.DtosDebiles.EstadoDto
{
    public class EstadoCreateDto
    {
        [Required]
        public string? NombreEstado { get; set; }
        [Required]
        public string? CodigoEstado { get; set; }

        // Llave foranea
        [Required]
        public int PaisId { get; set; }
    }
}
