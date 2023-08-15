namespace Domain.DTO.DtosDebiles.ProductoDto
{
    public class ProductoCreateDto
    {
        [Required]
        public string? NombreProducto { get; set; }
        [Required]
        public int CategoriaId { get; set; }
        [Required]
        public int MarcaId { get; set; }
    }
}
