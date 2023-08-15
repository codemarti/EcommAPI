namespace Domain.DTO.DtosDebiles.ProductoDto
{
    public class ProductoGetDto
    {
        [Required]
        public int IdProducto { get; set; }
        [Required]
        public string? NombreProducto { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public Categoria? Categoria { get; set; }
        public Marca? Marca { get; set; }
    }
}
