namespace Domain.DTO.DtosDebiles.ContactoEmpleadoDto
{
    public class ContactoEmpleadoCreateDto
    {
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
