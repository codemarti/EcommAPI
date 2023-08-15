namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly DatosDbContext _context;

        public ProductoRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Producto> Actualizar(Producto entidad)
        {
            entidad.FechaActualizacion = DateTime.UtcNow;
            _context.Productos.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}