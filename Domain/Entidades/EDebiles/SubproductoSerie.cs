namespace Domain.Entidades.EDebiles
{
    public class SubproductoSerie
    {
        [Key]
        public int IdSerie { get; set; }
        public string? NumeroSerie { get; set; }
        public Subproducto? Subproducto { get; set; }
        [ForeignKey("Subproducto")]
        public int? SubproductoId { get; set; }
    }
}
