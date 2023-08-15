namespace Domain.DTO.DtosFuertes.EstadoPagoDto
{
    public class EstadoPagoCreateDto
    {
        [Required]
        [MaxLength(45)]
        public string? NombreEdoPago { get; set; }
    }
}
