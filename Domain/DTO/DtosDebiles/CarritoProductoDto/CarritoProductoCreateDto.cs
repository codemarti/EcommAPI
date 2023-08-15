namespace Domain.DTO.DtosDebiles.CarritoProductoDto
{
    public class CarritoProductoCreateDto
    {
        // Cantidad de productos
        [Required]
        public int Cantidad { get; set; }
        // Llaves foraneas
        [Required]
        public int SubproductoId { get; set; }
    }
}
