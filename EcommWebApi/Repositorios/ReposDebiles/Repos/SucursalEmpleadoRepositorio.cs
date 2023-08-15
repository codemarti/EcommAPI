namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class SucursalEmpleadoRepositorio : Repositorio<SucursalEmpleado>, ISucursalEmpleadoRepositorio
    {
        private readonly DatosDbContext _context;

        public SucursalEmpleadoRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SucursalEmpleado> Actualizar(SucursalEmpleado entidad)
        {
            entidad.FechaIngreso = DateTime.UtcNow;
            _context.SucursalesEmpleados.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
