namespace Domain.DTO.DtosFuertes.EstadoSucursalDto
{
    public class EstadoSucursalCreateDto
    {
        [Required]
        [MaxLength(45)]
        public string? NombreEdoSucursal { get; set; }
    }
}
