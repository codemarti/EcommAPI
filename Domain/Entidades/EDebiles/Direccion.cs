namespace Domain.Entidades.EDebiles
{
    public class Direccion
    {
        [Key]
        public int IdDireccion { get; set; }
        public string? CP { get; set; }
        public string? Calle1 { get; set; }
        public string? Calle2 { get; set; }
        public string? NumExt { get; set; }
        public string? Detalles { get; set; }

        // Llave foranea
        public Ciudad? Ciudad { get; set; }
        [ForeignKey("Ciudad")]
        public int? CiudadId { get; set; }
    }
}
