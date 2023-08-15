namespace Domain.Entidades.EDebiles
{
    public class Estado
    {
        [Key]
        public int IdEstado { get; set; }
        public string? NombreEstado { get; set; }
        public string? CodigoEstado { get; set; }

        // Llave foranea
        public Pais? Pais { get; set; }
        [ForeignKey("Pais")]
        public int? PaisId { get; set; }
    }
}
