namespace Domain.DTO.DtosHistorial
{
    public class HistorialSubproductoGetDto
    {
        [Required]
        public int IdHistorialSubproducto { get; set; }
        [Required]
        public string? FechaRealizado { get; set; }
        [Required]
        public string? NombreAccion { get; set; }
        [Required]
        public string? NombreEmpleado { get; set; }
        [Required]
        public string? CodigoBarrasAntiguo { get; set; }
        [Required]
        public string? CodigoBarrasNuevo { get; set; }
        [Required]
        public string? ImagenAntigua { get; set; }
        [Required]
        public string? ImagenNueva { get; set; }
        [Required]
        public string? DescripcionAntigua { get; set; }
        [Required]
        public string? DescripcionNueva { get; set; }
        [Required]
        public string? PrecioSubAntiguo { get; set; }
        [Required]
        public string? PrecioSubNuevo { get; set; }
        [Required]
        public string? StockAntiguo { get; set; }
        [Required]
        public string? StockNuevo { get; set; }
        [Required]
        public string? NombreProductoAntiguo { get; set; }
        [Required]
        public string? NombreProductoNuevo { get; set; }
        [Required]
        public string? NombreSucursalAntigua { get; set; }
        [Required]
        public string? NombreSucursalNueva { get; set; }
        [Required]
        public string? CarritoProductoAfectado { get; set; }
        [Required]
        public string? SubproductoSerieAfectado { get; set; }
    }
}
