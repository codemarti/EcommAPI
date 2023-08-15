namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class CarritoCompraRepositorio : Repositorio<CarritoCompra>, ICarritoCompraRepositorio
    {
        private readonly DatosDbContext _context;

        public CarritoCompraRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CarritoCompra> Actualizar(CarritoCompra entidad)
        {
            entidad.Impuestos = entidad.CarritoProducto.Subtotal * (20M / 100);
            entidad.Total = entidad.CarritoProducto.Subtotal + entidad.Impuestos - entidad.Descuento;
            _context.CarritoCompras.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
