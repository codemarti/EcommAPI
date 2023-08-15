namespace Domain.DTO.DtosDebiles.TipoCadenaDto
{
    public class TipoCadenaGetDto
    {
        [Required]
        public int IdTipoCadena { get; set; }
        [Required]
        public string? NombreTipoCadena { get; set; }
        public string? Descripcion { get; set; }

        // Llave foranea
        [Required]
        public ModeloNegocio? ModeloNegocio { get; set; }
    }
}
