namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class CiudadRepositorio : Repositorio<Ciudad>, ICiudadRepositorio
    {
        private readonly DatosDbContext _context;

        public CiudadRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Ciudad> Actualizar(Ciudad entidad)
        {
            _context.Ciudades.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
