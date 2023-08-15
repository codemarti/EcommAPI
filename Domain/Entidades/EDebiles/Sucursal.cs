namespace Domain.Entidades.EDebiles
{
    public class Sucursal
    {
        [Key]
        public int IdSucursal { get; set; }
        // Atributo: RFC de la sucursal
        [Required]
        public string? RFCS { get; set; }
        public string? NombreSucursal { get; set; }
        public string? TelSucursal { get; set; }
        public DateTime? FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string? ImagenSucursal { get; set; }
        public string? PPS { get; set; }
        public string? TCS { get; set; }
        public string? Detalles { get; set; }

        // Llaves foraneas
        public Cadena? Cadena { get; set; }
        [ForeignKey("Cadena")]
        public int? CadenaId { get; set; }
        public Direccion? Direccion { get; set; }
        [ForeignKey("Direccion")]
        public int? DireccionId { get; set; }
        public EstadoSucursal? EstadoSucursal { get; set; }
        [ForeignKey("EstadoSucursal")]
        public int? EdoSucursalId { get; set; }
        public HorarioSucursal? HorarioSucursal { get; set; }
        [ForeignKey("HorarioSucursal")]
        public int? HorarioId { get; set; }
        public ServiciosSucursal? ServiciosSucursal { get; set; }
        [ForeignKey("ServiciosSucursal")]
        public int? ServicioId { get; set; }
    }
}
