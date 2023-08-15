namespace Domain.DTO.DtosDebiles.SubproductoSerieDto
{
    public class SubproductoSerieUpdateDto
    {
        [Required]
        public int IdSerie { get; set; }
        [Required]
        public string? NumeroSerie { get; set; }
        [Required]
        public int SubproductoId { get; set; }
    }
}
