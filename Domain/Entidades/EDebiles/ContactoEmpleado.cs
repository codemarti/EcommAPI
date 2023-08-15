namespace Domain.Entidades.EDebiles
{
    public class ContactoEmpleado
    {
        [Key]
        public int IdContacto { get; set; }
        [Required]
        public string NombreContacto { get; set; } = string.Empty;
        [Required]
        public string Telefono { get; set; } = string.Empty;
        [Required]
        public string Parentesco { get; set; } = string.Empty;
        public Empleado? Empleado { get; set; }
        [ForeignKey("Empleado")]
        public int? EmpleadoId { get; set; }
    }
}
