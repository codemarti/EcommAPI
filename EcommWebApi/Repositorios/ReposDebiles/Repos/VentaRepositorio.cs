namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class VentaRepositorio : Repositorio<Venta>, IVentaRepositorio
    {
        private readonly DatosDbContext _context;

        public VentaRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Venta> Actualizar(Venta entidad)
        {
            _context.Ventas.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
