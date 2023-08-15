namespace Domain.DTO.DtosDebiles.CarritoProductoDto
{
    public class CarritoProductoGetDto
    {
        [Required]
        public int IdCarritoProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime? FechaCreacionCarrito { get; set; }
        public DateTime? FechaActualizacionCarrito { get; set; }
        // Llaves foraneas
        [Required]
        public Subproducto? Subproducto { get; set; }
    }
}
