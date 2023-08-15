namespace Domain.DTO.DtosDebiles.UsuarioDto
{
    public class UsuarioGetDto
    {
        [Required]
        public int IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? ApellidosUsuario { get; set; }
        public string? TelUsuario { get; set; }
        public DateTime? FechaNacUsuario { get; set; }
        [Required]
        public string? Nickname { get; set; }
        public string? EmailUsuario { get; set; }
        public string? PasswordUsuario { get; set; }

        // Llaves foraneas
        [Required]
        public Direccion? Direccion { get; set; }
        [Required]
        public EstadoCuenta? EstadoCuenta { get; set; }
        [Required]
        public Genero? Genero { get; set; }
    }
}
