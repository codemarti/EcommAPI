namespace Domain.DTO.DtosFuertes.EstadoCarritoDto
{
    public class EstadoCarritoCreateDto
    {
        [Required]
        [MaxLength(45)]
        public string? NombreEdoCarrito { get; set; }
    }
}
