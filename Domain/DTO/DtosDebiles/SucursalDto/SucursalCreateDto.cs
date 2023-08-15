namespace Domain.DTO.DtosDebiles.SucursalDto
{
    public class SucursalCreateDto
    {
        [Required]
        public string? RFCS { get; set; }
        [Required]
        public string? NombreSucursal { get; set; }
        [Required]
        public string? TelSucursal { get; set; }
        public DateTime? FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string? ImagenSucursal { get; set; }
        [Required]
        public string? PPS { get; set; }
        [Required]
        public string? TCS { get; set; }
        public string? Detalles { get; set; }

        // Llaves foraneas
        [Required]
        public int CadenaId { get; set; }
        [Required]
        public int DireccionId { get; set; }
        [Required]
        public int EdoSucursalId { get; set; }
        [Required]
        public int HorarioId { get; set; }
        [Required]
        public int ServicioId { get; set; }
    }
}
