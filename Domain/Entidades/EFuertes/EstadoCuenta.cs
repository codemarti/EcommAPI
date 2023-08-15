namespace Domain.Entidades.EFuertes
{
    public class EstadoCuenta
    {
        [Key]
        public int IdEdoCuenta { get; set; }
        public string? NombreEdoCuenta { get; set; }
    }
}
