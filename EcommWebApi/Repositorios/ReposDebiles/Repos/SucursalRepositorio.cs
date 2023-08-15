namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class SucursalRepositorio : Repositorio<Sucursal>, ISucursalRepositorio
    {
        private readonly DatosDbContext _context;

        public SucursalRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Sucursal> Actualizar(Sucursal entidad)
        {
            _context.Sucursales.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
