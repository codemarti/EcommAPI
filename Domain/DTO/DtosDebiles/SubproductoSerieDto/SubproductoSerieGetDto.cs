namespace Domain.DTO.DtosDebiles.SubproductoSerieDto
{
    public class SubproductoSerieGetDto
    {
        [Required]
        public int IdSerie { get; set; }
        [Required]
        public string? NumeroSerie { get; set; }
        [Required]
        public Subproducto? Subproducto { get; set; }
    }
}
