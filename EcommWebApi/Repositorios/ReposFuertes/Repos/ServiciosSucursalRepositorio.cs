namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class ServiciosSucursalRepositorio : Repositorio<ServiciosSucursal>, IServiciosSucursalRepositorio
    {
        private readonly DatosDbContext _context;

        public ServiciosSucursalRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ServiciosSucursal> Actualizar(ServiciosSucursal entidad)
        {
            _context.ServiciosSucursales.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
