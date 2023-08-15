namespace Domain.DTO.DtosFuertes.MetodoPagoDto
{
    public class MetodoPagoCreateDto
    {
        [Required]
        [MaxLength(45)]
        public string? NombreMetodoPago { get; set; }
        // Una imagen que represente al icono
        public string? Icono { get; set; }
        // Momento en el que fue agregada, unicamente para los desarrolladores
        public DateTime FechaCreacion { get; set; }
    }
}
