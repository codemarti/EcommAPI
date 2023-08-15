namespace Domain.DTO.DtosDebiles.CarritoCompraDto
{
    public class CarritoCompraCreateDto
    {
        public string? CodigoCarrito { get; set; }
        public decimal Descuento { get; set; }
        //public decimal Impuestos { get; set; }
        //public decimal Total { get; set; }
        // Llaves foraneas
        [Required]
        public int CarritoProductoId { get; set; }
        [Required]
        public int EdoCarritoId { get; set; }
        [Required]
        public int PagoId { get; set; }
        [Required]
        public int UsuarioId { get; set; }
    }
}
