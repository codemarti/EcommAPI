namespace Domain.DTO.DtosDebiles.ContactoEmpleadoDto
{
    public class ContactoEmpleadoGetDto
    {
        [Required]
        public int IdContacto { get; set; }
        [Required]
        public string NombreContacto { get; set; } = string.Empty;
        [Required]
        public string Telefono { get; set; } = string.Empty;
        [Required]
        public string Parentesco { get; set; } = string.Empty;
        [Required]
        public Empleado? Empleado { get; set; }
    }
}
