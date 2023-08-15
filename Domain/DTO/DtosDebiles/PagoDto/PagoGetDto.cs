namespace Domain.DTO.DtosDebiles.PagoDto
{
    public class PagoGetDto
    {
        [Required]
        public int IdPago { get; set; }
        [Required]
        public string? NombreTitular { get; set; }
        [Required]
        public string? NumeroTarjeta { get; set; }
        [Required]
        public string? CVV { get; set; }
        [Required]
        public DateTime? FechaExpiracion { get; set; }
        // Este dato no se mostrara al cliente solo se guardara en la base de datos el dia de de su creacion de esta tarjeta
        //public DateTime FechaCreacion { get; set; }
        [Required]
        public MetodoPago? MetodoPago { get; set; }
    }
}
