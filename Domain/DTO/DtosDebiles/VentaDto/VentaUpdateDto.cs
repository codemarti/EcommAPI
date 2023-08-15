namespace Domain.DTO.DtosDebiles.VentaDto
{
    public class VentaUpdateDto
    {
        [Required]
        public int IdVenta { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string? Comentarios { get; set; }

        // Llaves foraneas
        [Required]
        public int CarritoId { get; set; }
        [Required]
        public int EmpleadoId { get; set; }
        [Required]
        public int EdoPagoId { get; set; }
    }
}
