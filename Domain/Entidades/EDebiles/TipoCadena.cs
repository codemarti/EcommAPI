namespace Domain.Entidades.EDebiles
{
    public class TipoCadena
    {
        [Key]
        public int IdTipoCadena { get; set; }
        public string? NombreTipoCadena { get; set; }
        public string? Descripcion { get; set; }

        // Llave foranea
        public ModeloNegocio? ModeloNegocio { get; set; }
        [ForeignKey("ModeloNegocio")]
        public int? ModeloId { get; set; }
    }
}
