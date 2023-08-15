namespace Domain.Entidades.EHistorial
{
    public class HistorialProducto
    {
        [Key]
        public int IdHistorialProducto { get; set; }
        public string? FechaRealizado { get; set; }
        public string? NombreAccion { get; set; }
        public string? NombreEmpleado { get; set; }
        public string? NombreCategoriaAntigua { get; set; }
        public string? NombreCategoriaNueva { get; set; }
        public string? NombreMarcaAntigua { get; set; }
        public string? NombreMarcaNueva { get; set; }
        public string? NombreProductoAntiguo { get; set; }
        public string? NombreProductoNuevo { get; set; }
        public string? SubproductoAfectado { get; set; }
    }
}