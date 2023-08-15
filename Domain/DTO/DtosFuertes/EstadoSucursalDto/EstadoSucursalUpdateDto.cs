namespace Domain.DTO.DtosFuertes.EstadoSucursalDto
{
    public class EstadoSucursalUpdateDto
    {
        [Required]
        public int IdEdoSucursal { get; set; }
        [Required]
        [MaxLength(45)]
        public string? NombreEdoSucursal { get; set; }
    }
}
