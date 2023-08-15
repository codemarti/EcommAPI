namespace Domain.DTO.DtosDebiles.ProductoDto
{
    public class ProductoUpdateDto
    {
        [Required]
        public int IdProducto { get; set; }
        [Required]
        public string? NombreProducto { get; set; }
        [Required]
        public int CategoriaId { get; set; }
        [Required]
        public int MarcaId { get; set; }
    }
}
