namespace Domain.DTO.DtosDebiles.SubproductoDto
{
    public class SubproductoUpdateDto
    {
        [Required]
        public int IdSubproducto { get; set; }
        [Required]
        public string? CodigoBarras { get; set; }
        public string? ImagenSub { get; set; }
        public string? Descripcion { get; set; }
        [Required]
        public decimal PrecioSub { get; set; }
        public int Stock { get; set; }
        [Required]
        public int ProductoId { get; set; }
        [Required]
        public int SucursalId { get; set; }
    }
}
