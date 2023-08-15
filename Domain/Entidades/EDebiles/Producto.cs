namespace Domain.Entidades.EDebiles
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        public string? NombreProducto { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        // Llaves foraneas
        public Categoria? Categoria { get; set; }
        [ForeignKey("Categoria")]
        public int? CategoriaId { get; set; }
        public Marca? Marca { get; set; }
        [ForeignKey("Marca")]
        public int? MarcaId { get; set; }
    }
}
