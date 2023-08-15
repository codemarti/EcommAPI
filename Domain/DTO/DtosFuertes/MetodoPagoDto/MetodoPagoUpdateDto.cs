namespace Domain.DTO.DtosFuertes.MetodoPagoDto
{
    public class MetodoPagoUpdateDto
    {
        [Required]
        public int IdMetodoPago { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombreMetodoPago { get; set; }
        public string? Icono { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
