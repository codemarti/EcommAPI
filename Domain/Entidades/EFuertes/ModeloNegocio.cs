namespace Domain.Entidades.EFuertes
{
    public class ModeloNegocio
    {
        [Key]
        public int IdModelo { get; set; }
        public string? NombreModelo { get; set; }
        public string? Descripcion { get; set; }
    }
}
