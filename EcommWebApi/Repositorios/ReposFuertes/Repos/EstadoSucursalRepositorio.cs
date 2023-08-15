namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class EstadoSucursalRepositorio : Repositorio<EstadoSucursal>, IEstadoSucursalRepositorio
    {
        private readonly DatosDbContext _context;

        public EstadoSucursalRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EstadoSucursal> Actualizar(EstadoSucursal entidad)
        {
            _context.EstadoSucursales.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
