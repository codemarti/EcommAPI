namespace Domain.DTO.DtosDebiles.EnvioDto
{
    public class EnvioUpdateDto
    {
        [Required]
        public string? ReferenciaEnvio { get; set; }

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
