namespace Domain.DTO.DtosFuertes.EstadoPagoDto
{
    public class EstadoPagoUpdateDto
    {
        [Required]
        public int IdEdoPago { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombreEdoPago { get; set; }
    }
}
