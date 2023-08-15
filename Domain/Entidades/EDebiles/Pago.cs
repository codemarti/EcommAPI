namespace Domain.Entidades.EDebiles
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }
        public string? NombreTitular { get; set; }
        public string? NumeroTarjeta { get; set; }
        public string? CVV { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        // Este dato no se mostrara al cliente solo se guardara en la base de datos el dia de de su cracion de esta tarjeta
        public DateTime? FechaCreacion { get; set; }
        public MetodoPago? MetodoPago { get; set; }
        [ForeignKey("MetodoPago")]
        public int? MetodoPagoId { get; set; }
    }
}
