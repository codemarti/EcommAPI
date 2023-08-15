namespace Domain.DTO.DtosDebiles.UsuarioDto
{
    public class UsuarioUpdateDto
    {
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public string? NombreUsuario { get; set; }
        [Required]
        public string? ApellidosUsuario { get; set; }
        [Required]
        public string? TelUsuario { get; set; }
        [Required]
        public DateTime? FechaNacUsuario { get; set; }
        [Required]
        public string? Nickname { get; set; }
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
