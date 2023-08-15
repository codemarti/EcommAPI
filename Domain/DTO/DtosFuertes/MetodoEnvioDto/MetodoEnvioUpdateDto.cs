namespace Domain.DTO.DtosFuertes.MetodoEnvioDto
{
    public class MetodoEnvioUpdateDto
    {
        [Required]
        public int IdMetodoEnvio { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombreMetodoEnvio { get; set; }
    }
}
