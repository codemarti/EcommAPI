namespace Domain.DTO.DtosDebiles.EnvioDto
{
    public class EnvioCreateDto
    {
        public string? ReferenciaEnvio { get; set; }

        // Llaves foraneas
        [Required]
        public int DireccionId { get; set; }
        [Required]
        public int EdoEnvioId { get; set; }
        [Required]
        public int MetodoEnvioId { get; set; }
        [Required]
        public int VentaId { get; set; }
    }
}
