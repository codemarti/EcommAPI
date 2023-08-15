namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class EstadoCuentaRepositorio : Repositorio<EstadoCuenta>, IEstadoCuentaRepositorio
    {
        private readonly DatosDbContext _context;

        public EstadoCuentaRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EstadoCuenta> Actualizar(EstadoCuenta entidad)
        {
            _context.EstadoCuentas.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
