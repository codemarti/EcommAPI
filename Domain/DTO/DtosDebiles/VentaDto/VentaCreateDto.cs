namespace Domain.DTO.DtosDebiles.VentaDto
{
    public class VentaCreateDto
    {
        // Se puede crear automaticamente?
        //public DateTime FechaRealizada { get; set; }
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
