namespace Domain.DTO.DtosHistorial
{
    public class HistorialProductoGetDto
    {
        [Required]
        public int IdHistorialProducto { get; set; }
        [Required]
        public string? FechaRealizado { get; set; }
        [Required]
        public string? NombreAccion { get; set; }
        [Required]
        public string? NombreEmpleado { get; set; }
        [Required]
        public string? NombreCategoriaAntigua { get; set; }
        [Required]
        public string? NombreCategoriaNueva { get; set; }
        [Required]
        public string? NombreMarcaAntigua { get; set; }
        [Required]
        public string? NombreMarcaNueva { get; set; }
        [Required]
        public string? NombreProductoAntiguo { get; set; }
        [Required]
        public string? NombreProductoNuevo { get; set; }
        [Required]
        public string? SubproductoAfectado { get; set; }
    }
}
