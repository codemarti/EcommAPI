namespace Domain.DTO.DtosDebiles.VentaDto
{
    public class VentaGetDto
    {
        [Required]
        public int IdVenta { get; set; }
        public DateTime? FechaRealizada { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string? Comentarios { get; set; }

        // Llaves foraneas
        [Required]
        public CarritoCompra? CarritoCompra { get; set; }
        [Required]
        public Empleado? Empleado { get; set; }
        [Required]
        public EstadoPago? EstadoPago { get; set; }
    }
}
