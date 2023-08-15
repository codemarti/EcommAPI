namespace Domain.DTO.DtosDebiles.SubproductoDto
{
    public class SubproductoCreateDto
    {
        [Required]
        [StringLength(13)]
        public string? CodigoBarras { get; set; }
        //[Required]
        //[StringLength(16)]
        //public string? NumSerie { get; set; }
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
