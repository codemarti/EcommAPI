namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class EmpleadoRepositorio : Repositorio<Empleado>, IEmpleadoRepositorio
    {
        private readonly DatosDbContext _context;

        public EmpleadoRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Empleado> Actualizar(Empleado entidad)
        {
            _context.Empleados.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
