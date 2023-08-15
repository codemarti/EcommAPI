namespace Domain.Entidades.EFuertes
{
    public class EstadoEmpleado
    {
        [Key]
        public int IdEdoEmpleado { get; set; }
        [Required]
        public string NombreEdoEmpleado { get; set; } = string.Empty;
    }
}
