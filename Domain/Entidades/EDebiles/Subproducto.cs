namespace Domain.Entidades.EDebiles
{
    public class Subproducto
    {
        [Key]
        public int IdSubproducto { get; set; }
        [Required]
        public string? CodigoBarras { get; set; }
        public string? ImagenSub { get; set; }
        public string? Descripcion { get; set; }
        [Required]
        public decimal PrecioSub { get; set; }
        public int Stock { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public Producto? Producto { get; set; }
        [ForeignKey("Producto")]
        public int? ProductoId { get; set; }
        public Sucursal? Sucursal { get; set; }
        [ForeignKey("Sucursal")]
        public int? SucursalId { get; set; }
    }
}
