namespace Domain.Entidades.EDebiles
{
    public class Envio
    {
        [Key]
        public int IdEnvio { get; set; }
        public string? ReferenciaEnvio { get; set; }

        // Llaves foraneas
        public Direccion? Direccion { get; set; }
        [ForeignKey("Direccion")]
        public int? DireccionId { get; set; }
        public MetodoEnvio? MetodoEnvio { get; set; }
        [ForeignKey("MetodoEnvio")]
        public int? MetodoEnvioId { get; set; }
        public Venta? Venta { get; set; }
        [ForeignKey("Venta")]
        public int? VentaId { get; set; }
    }
}
