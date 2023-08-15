namespace Domain.DTO.DtosFuertes.MetodoPagoDto
{
    public class MetodoPagoGetDto
    {
        public int IdMetodoPago { get; set; }
        public string? NombreMetodoPago { get; set; }
        public string? Icono { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
