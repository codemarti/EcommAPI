namespace Domain.DTO.DtosDebiles.UsuarioDto
{
    public class UsuarioCreateDto
    {
        [Required]
        public string? NombreUsuario { get; set; }
        [Required]
        public string? ApellidosUsuario { get; set; }
        public string? TelUsuario { get; set; }
        [Required]
        public DateTime? FechaNacUsuario { get; set; }
        [Required]
        public string? Nickname { get; set; }
        [Required]
        public string? EmailUsuario { get; set; }
        [Required]
        public string? PasswordUsuario { get; set; }

        // Llaves foraneas
        [Required]
        public int DireccionId { get; set; }
        [Required]
        public int EdoCuentaId { get; set; }
        [Required]
        public int GeneroId { get; set; }
    }
}
