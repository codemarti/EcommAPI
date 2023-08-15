namespace Domain.DTO.DtosDebiles.CarritoCompraDto
{
    public class CarritoCompraUpdateDto
    {
        [Required]
        public int IdCarrito { get; set; }
        public decimal Descuento { get; set; }
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
