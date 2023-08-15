namespace Domain.Entidades.EFuertes
{
    public class Pais
    {
        [Key]
        public int IdPais { get; set; }
        public string? NombrePais { get; set; }
        public string? ISO { get; set; }
    }
}
