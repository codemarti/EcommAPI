namespace Domain.Entidades.EHistorial
{
    public class HistorialSubproducto
    {
		[Key]
		public int IdHistorialSubproducto { get; set; }
		public string? FechaRealizado { get; set; }
		public string? NombreAccion { get; set; }
		public string? NombreEmpleado { get; set; }
		public string? CodigoBarrasAntiguo { get; set; }
		public string? CodigoBarrasNuevo { get; set; }
		public string? ImagenAntigua { get; set; }
		public string? ImagenNueva { get; set; }
		public string? DescripcionAntigua { get; set; }
		public string? DescripcionNueva { get; set; }
		public string? PrecioSubAntiguo { get; set; }
		public string? PrecioSubNuevo { get; set; }
		public string? StockAntiguo { get; set; }
		public string? StockNuevo { get; set; }
		public string? NombreProductoAntiguo { get; set; }
		public string? NombreProductoNuevo { get; set; }
		public string? NombreSucursalAntigua { get; set; }
		public string? NombreSucursalNueva { get; set; }
		public string? CarritoProductoAfectado { get; set; }
		public string? SubproductoSerieAfectado { get; set; }
    }
}