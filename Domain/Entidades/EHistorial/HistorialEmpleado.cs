namespace Domain.Entidades.EHistorial
{
    public class HistorialEmpleado
    {
        [Key]
        public int IdHistorialEmpleado { get; set; }
        public string? FechaRealizado { get; set; }
        public string? NombreAccion { get; set; }
        public string? NombrePersona { get; set; }
        public string? ApellidoEmpleadoAntiguo { get; set; }
        public string? ApellidoEmpleadoNuevo { get; set; }
        public string? NombreEmpleadoAntiguo { get; set; }
        public string? NombreEmpleadoNuevo { get; set; }
        public string? NumeroEmpleadoAntiguo { get; set; }
        public string? NumeroEmpleadoNuevo { get; set; }
        public string? RFCAntiguo { get; set; }
        public string? RFCNuevo { get; set; }
        public string? FechaNacimientoAntigua { get; set; }
        public string? FechaNacimientoNueva { get; set; }
        public string? TelefonoEmpleadoAntiguo { get; set; }
        public string? TelefonoEmpleadoNuevo { get; set; }
        public string? EmailEmpleadoAntiguo { get; set; }
        public string? EmailEmpleadoNuevo { get; set; }
        public string? NombreUsuarioAntiguo { get; set; }
        public string? NombreUsuarioNuevo { get; set; }
        public string? PasswordEmpleadoAntigua { get; set; }
        public string? PasswordEmpleadoNueva { get; set; }
        public string? FechaIngresoAntigua { get; set; }
        public string? FechaIngresoNueva { get; set; }
        public string? FechaBajaAntigua { get; set; }
        public string? FechaBajaNueva { get; set; }
        public string? DireccionEmpleadoAntigua { get; set; }
        public string? DireccionEmpleadoNueva { get; set; }
        public string? NombreEdoEmpleadoAntiguo { get; set; }
        public string? NombreEdoEmpleadoNuevo { get; set; }
        public string? NombreGeneroAntiguo { get; set; }
        public string? NombreGeneroNuevo { get; set; }
        public string? NombreRolAntiguo { get; set; }
        public string? NombreRolNuevo { get; set; }
    }
}
