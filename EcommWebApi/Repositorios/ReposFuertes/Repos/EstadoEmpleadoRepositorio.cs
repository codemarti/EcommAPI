namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class EstadoEmpleadoRepositorio : Repositorio<EstadoEmpleado>, IEstadoEmpleadoRepositorio
    {
        private readonly DatosDbContext _context;

        public EstadoEmpleadoRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EstadoEmpleado> Actualizar(EstadoEmpleado entidad)
        {
            _context.EstadoEmpleados.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
