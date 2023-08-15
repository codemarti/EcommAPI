namespace Domain.DTO.DtosFuertes.MetodoEnvioDto
{
    public class MetodoEnvioCreateDto
    {
        [Required]
        [MaxLength(45)]
        public string? NombreMetodoEnvio { get; set; }
    }
}
