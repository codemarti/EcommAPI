namespace Domain.DTO.DtosDebiles.SucursalEmpleadoDto
{
    public class SucursalEmpleadoUpdateDto
    {
        [Required]
        public int IdSucursalEmpleado { get; set; }
        [Required]
        public int EmpleadoId { get; set; }
        [Required]
        public int SucursalId { get; set; }
    }
}
