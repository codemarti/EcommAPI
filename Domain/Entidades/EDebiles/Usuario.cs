namespace Domain.Entidades.EDebiles
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? ApellidosUsuario { get; set; }
        public string? TelUsuario { get; set; }
        public DateTime? FechaNacUsuario { get; set; }
        public string? Nickname { get; set; }
        public string? EmailUsuario { get; set; }
        public string? PasswordUsuario { get; set; }
        // Dato posiblemente unico para los desarrolladores
        public DateTime? FechaRegistro { get; set; }

        // Llaves foraneas
        public Direccion? Direccion { get; set; }
        [ForeignKey("Direccion")]
        public int? DireccionId { get; set; }
        public EstadoCuenta? EstadoCuenta { get; set; }
        [ForeignKey("EstadoCuenta")]
        public int? EdoCuentaId { get; set; }
        public Genero? Genero { get; set; }
        [ForeignKey("Genero")]
        public int? GeneroId { get; set; }
    }
}
