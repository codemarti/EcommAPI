namespace Domain.DTO.DtosFuertes.EstadoEmpleadoDto
{
    public class EstadoEmpleadoGetDto
    {
        [Required]
        public int IdEdoEmpleado { get; set; }
        [Required]
        public string NombreEdoEmpleado { get; set; } = string.Empty;
    }
}
