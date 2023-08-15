namespace Domain.DTO.DtosFuertes.EstadoCarritoDto
{
    public class EstadoCarritoUpdateDto
    {
        [Required]
        public int IdEdoCarrito { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombreEdoCarrito { get; set; }
    }
}
