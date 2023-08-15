namespace Domain.Entidades.EDebiles
{
    public class Venta
    {
        [Key]
        public int IdVenta { get; set; }
        public DateTime? FechaRealizada { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string? Comentarios { get; set; }

        // Llaves foraneas
        public CarritoCompra? CarritoCompra { get; set; }
        [ForeignKey("CarritoCompra")]
        public int? CarritoId { get; set; }
        public Empleado? Empleado { get; set; }
        [ForeignKey("Empleado")]
        public int? EmpleadoId { get; set; }
        public EstadoPago? EstadoPago { get; set; }
        [ForeignKey("EstadoPago")]
        public int? EdoPagoId { get; set; }
    }
}
