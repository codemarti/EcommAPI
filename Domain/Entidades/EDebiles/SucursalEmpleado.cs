namespace Domain.Entidades.EDebiles
{
    // Esta tabla llevara llaves foraneas, debido a que en este apartado se checan que
    // que empleados estan en que sucursal y viceversa, tambien se necesitara la fecha de ingreso a alguna sucursal
    public class SucursalEmpleado
    {
        [Key]
        public int IdSucursalEmpleado { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public Empleado? Empleado { get; set; }
        [ForeignKey("Empleado")]
        public int? EmpleadoId { get; set; }
        public Sucursal? Sucursal { get; set; }
        [ForeignKey("Sucursal")]
        public int? SucursalId { get; set; }
    }
}
