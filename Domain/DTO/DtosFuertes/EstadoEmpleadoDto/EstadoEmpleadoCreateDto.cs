namespace Domain.DTO.DtosFuertes.EstadoEmpleadoDto
{
    public class EstadoEmpleadoCreateDto
    {
        [Required]
        public string NombreEdoEmpleado { get; set; } = string.Empty;
    }
}
