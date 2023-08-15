namespace Domain.DTO.DtosDebiles.ContactoEmpleadoDto
{
    public class ContactoEmpleadoUpdateDto
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
        public int EmpleadoId { get; set; }
    }
}
