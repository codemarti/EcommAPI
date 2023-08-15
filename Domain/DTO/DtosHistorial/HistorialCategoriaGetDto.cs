namespace Domain.DTO.DtosHistorial
{
    public class HistorialCategoriaGetDto
    {
        [Required]
        public int IdHistorialCategoria { get; set; }
        [Required]
        public string? FechaRealizado { get; set; }
        [Required]
        public string? NombreAccion { get; set; }
        [Required]
        public string? NombreCategoriaAntigua { get; set; }
        [Required]
        public string? NombreCategoriaNueva { get; set; }
        [Required]
        public string? NombreEmpleado { get; set; }
        [Required]
        public string? ProductoAfectado { get; set; }
    }
}
