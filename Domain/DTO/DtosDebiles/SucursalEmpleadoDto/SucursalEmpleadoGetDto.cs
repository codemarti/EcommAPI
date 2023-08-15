namespace Domain.DTO.DtosDebiles.SucursalEmpleadoDto
{
    public class SucursalEmpleadoGetDto
    {
        [Required]
        public int IdSucursalEmpleado { get; set; }
        [Required]
        public DateTime? FechaIngreso { get; set; }
        [Required]
        public Empleado? Empleado { get; set; }
        [Required]
        public Sucursal? Sucursal { get; set; }
    }
}
