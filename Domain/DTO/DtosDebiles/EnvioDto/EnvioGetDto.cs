namespace Domain.DTO.DtosDebiles.EnvioDto
{
    public class EnvioGetDto
    {
        [Required]
        public int IdEnvio { get; set; }
        public string? ReferenciaEnvio { get; set; }

        // Llaves foraneas
        [Required]
        public Direccion? Direccion { get; set; }
        [Required]
        public MetodoEnvio? MetodoEnvio { get; set; }
        [Required]
        public Venta? Venta { get; set; }
    }
}
