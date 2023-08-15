namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class CarritoProductoRepositorio : Repositorio<CarritoProducto>, ICarritoProductoRepositorio
    {
        private readonly DatosDbContext _context;

        public CarritoProductoRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CarritoProducto> Actualizar(CarritoProducto entidad)
        {
            entidad.FechaActualizacionCarrito = DateTime.UtcNow;
            entidad.Subtotal = entidad.Cantidad * entidad.Subproducto.PrecioSub;
            _context.CarritoProductos.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
