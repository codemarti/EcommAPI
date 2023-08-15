namespace Domain.DTO.DtosDebiles.EstadoDto
{
    public class EstadoUpdateDto
    {
        [Required]
        public int IdEstado { get; set; }
        [Required]
        public string? NombreEstado { get; set; }
        [Required]
        public string? CodigoEstado { get; set; }

        // Llave foranea
        [Required]
        public int PaisId { get; set; }
    }
}
