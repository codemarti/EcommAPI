namespace Domain.Entidades.EFuertes
{
    public class MetodoPago
    {
        [Key]
        public int IdMetodoPago { get; set; }
        public string? NombreMetodoPago { get; set; }
        public string? Icono { get; set; }
        // Momento en el que fue agregada, unicamente para los desarrolladores
        public DateTime FechaCreacion { get; set; }
    }
}
