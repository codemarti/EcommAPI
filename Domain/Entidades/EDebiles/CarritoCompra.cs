namespace Domain.Entidades.EDebiles
{
    public class CarritoCompra
    {
        [Key]
        public int IdCarrito { get; set; }
        public string? CodigoCarrito { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }
        // Llaves foraneas
        public CarritoProducto? CarritoProducto { get; set; }
        [ForeignKey("CarritoProducto")]
        public int? CarritoProductoId { get; set; }
        public EstadoCarrito? EstadoCarrito { get; set; }
        [ForeignKey("EstadoCarrito")]
        public int? EdoCarritoId { get; set; }
        public Pago? Pago { get; set; }
        [ForeignKey("Pago")]
        public int? PagoId { get; set; }
        public Usuario? Usuario { get; set; }
        [ForeignKey("Usuario")]
        public int? UsuarioId { get; set; }
    }
}
