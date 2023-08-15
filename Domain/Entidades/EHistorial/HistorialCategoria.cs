namespace Domain.Entidades.EHistorial
{
    public class HistorialCategoria
    {
        [Key]
        public int IdHistorialCategoria { get; set; }
        public string? FechaRealizado { get; set; }
        public string? NombreAccion { get; set; }
        public string? NombreCategoriaAntigua { get; set; }
        public string? NombreCategoriaNueva { get; set; }
        public string? NombreEmpleado { get; set; }
        public string? ProductoAfectado { get; set; }
    }
}
