namespace Domain.DTO.DtosDebiles.SubproductoSerieDto
{
    public class SubproductoSerieCreateDto
    {
        [Required]
        public string? NumeroSerie { get; set; }
        [Required]
        public int SubproductoId { get; set; }
    }
}
