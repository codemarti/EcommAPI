namespace Domain.DTO.DtosDebiles.SucursalDto
{
    public class SucursalGetDto
    {
        [Required]
        public int IdSucursal { get; set; }
        // Atributo: RFC de la sucursal
        [Required]
        public string? RFCS { get; set; }
        [Required]
        public string? NombreSucursal { get; set; }
        public string? TelSucursal { get; set; }
        public DateTime? FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string? ImagenSucursal { get; set; }
        public string? PPS { get; set; }
        public string? TCS { get; set; }
        public string? Detalles { get; set; }

        // Llaves foraneas
        [Required]
        public Cadena? Cadena { get; set; }
        [Required]
        public Direccion? Direccion { get; set; }
        [Required]
        public EstadoSucursal? EstadoSucursal { get; set; }
        [Required]
        public HorarioSucursal? Horario { get; set; }
        [Required]
        public ServiciosSucursal? ServiciosSucursal { get; set; }
    }
}
