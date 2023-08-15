namespace Domain.Entidades.EFuertes
{
    public class ServiciosSucursal
    {
        [Key]
        public int IdServicio { get; set; }
        public string? NombreServicio { get; set; }
        public string? Decripcion { get; set; }
    }
}
