namespace Domain.Entidades.EDebiles
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }
        // Atributo: RFC del empleado
        [Required]
        public string? RFCE { get; set; }
        [Required]
        public string? NumEmpleado { get; set; }
        public string? NombreEmpleado { get; set; }
        public string? ApellidoEmpleado { get; set; }
        public string? TelEmpleado { get; set; }
        public DateTime? FechaNacEmpleado { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string? NickEmpleado { get; set; }
        public string? EmailEmpleado { get; set; }
        public string? PassEmpleado { get; set; }

        // Llaves foraneas
        public Direccion? Direccion { get; set; }
        [ForeignKey("Direccion")]
        public int? DireccionId { get; set; }
        public EstadoEmpleado? EstadoEmpleado { get; set; }
        [ForeignKey("EstadoEmpleado")]
        public int? EdoEmpleadoId { get; set; }
        public Genero? Genero { get; set; }
        public int? GeneroId { get; set; }
        public Rol? Rol { get; set; }
        [ForeignKey("Rol")]
        public int? RolId { get; set; }
    }
}
