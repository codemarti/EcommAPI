namespace Domain.DTO.DtosDebiles.CarritoProductoDto
{
    public class CarritoProductoUpdateDto
    {
        [Required]
        public int IdCarritoProducto { get; set; }
        // Cantidad de productos
        [Required]
        public int Cantidad { get; set; }
        // Llaves foraneas
        [Required]
        public int SubproductoId { get; set; }
    }
}
