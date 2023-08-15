namespace Domain.Entidades.EDebiles
{
    public class CarritoProducto
    {
        [Key]
        public int IdCarritoProducto { get; set; }
        // Cantidad de productos
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        // Estas propiedades seran
        // trabajadas de manera interna
        public DateTime? FechaCreacionCarrito { get; set; }
        public DateTime? FechaActualizacionCarrito { get; set; }

        // Llaves foraneas
        public Subproducto? Subproducto { get; set; }
        [ForeignKey("Subproducto")]
        public int? SubproductoId { get; set; }
    }
}
