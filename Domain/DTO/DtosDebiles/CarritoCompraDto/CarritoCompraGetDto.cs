namespace Domain.DTO.DtosDebiles.CarritoCompraDto
{
    public class CarritoCompraGetDto
    {
        [Required]
        public int IdCarrito { get; set; }
        [Required]
        public string? CodigoCarrito { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }
        // Llaves foraneas
        [Required]
        public CarritoProducto? CarritoProducto { get; set; }
        [Required]
        public EstadoCarrito? EstadoCarrito { get; set; }
        [Required]
        public Pago? Pago { get; set; }
        [Required]
        public Usuario? Usuario { get; set; }
    }
}
