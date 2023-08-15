namespace Domain.DTO.DtosFuertes.EstadoEmpleadoDto
{
    public class EstadoEmpleadoUpdateDto
    {
        [Required]
        public int IdEdoEmpleado { get; set; }
        [Required]
        public string NombreEdoEmpleado { get; set; } = string.Empty;
    }
}
