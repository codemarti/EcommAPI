namespace Domain.DTO.DtosDebiles.EmpleadoDto
{
    public class EmpleadoUpdateDto
    {
        [Required]
        public int IdEmpleado { get; set; }
        // Atributo: RFC del empleado
        [Required]
        public string? RFCE { get; set; }
        [Required]
        public string? NumEmpleado { get; set; }
        [Required]
        public string? NombreEmpleado { get; set; }
        [Required]
        public string? ApellidoEmpleado { get; set; }
        [Required]
        public string? TelEmpleado { get; set; }
        [Required]
        public DateTime? FechaNacEmpleado { get; set; }
        [Required]
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string? NickEmpleado { get; set; }
        [Required]
        public string? PassEmpleado { get; set; }

        // Laves foraneas
        [Required]
        public int DireccionId { get; set; }
        [Required]
        public int EdoEmpleadoId { get; set; }
        [Required]
        public int GeneroId { get; set; }
        [Required]
        public int RolId { get; set; }
    }
}
