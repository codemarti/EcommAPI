namespace Domain.DTO.DtosDebiles.EmpleadoDto
{
    public class EmpleadoGetDto
    {
        [Required]
        public int IdEmpleado { get; set; }
        // Atributo: RFC del empleado
        [Required]
        public string? RFCE { get; set; }
        [Required]
        public string? NombreEmpleado { get; set; }
        [Required]
        public string? NumEmpleado { get; set; }
        [Required]
        public string? ApellidoEmpleado { get; set; }
        [Required]
        public string? TelEmpleado { get; set; }
        [Required]
        public DateTime? FechaNacEmpleado { get; set; }
        [Required]
        public DateTime? FechaIngreso { get; set; }
        [Required]
        public DateTime? FechaBaja { get; set; }
        [Required]
        public string? NickEmpleado { get; set; }
        [Required]
        public string? EmailEmpleado { get; set; }
        public string? PassEmpleado { get; set; }

        // Laves foraneas
        [Required]
        public Direccion? Direccion { get; set; }
        [Required]
        public EstadoEmpleado? EstadoEmpleado { get; set; }
        [Required]
        public Genero? Genero { get; set; }
        [Required]
        public Rol? Rol { get; set; }
    }
}
