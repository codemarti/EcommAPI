namespace Domain.Entidades.EDebiles
{
    public class Ciudad
    {
        [Key]
        public int IdCiudad { get; set; }
        public string? NombreCiudad { get; set; }

        // Llave foranea
        public Estado? Estado { get; set; }
        [ForeignKey("Estado")]
        public int? EstadoId { get; set; }
    }
}
