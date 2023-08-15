namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class DireccionRepositorio : Repositorio<Direccion>, IDireccionRepositorio
    {
        private readonly DatosDbContext _context;

        public DireccionRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Direccion> Actualizar(Direccion entidad)
        {
            _context.Direcciones.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
