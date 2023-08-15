namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class EstadoRepositorio : Repositorio<Estado>, IEstadoRepositorio
    {
        private readonly DatosDbContext _context;

        public EstadoRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Estado> Actualizar(Estado entidad)
        {
            _context.Estados.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
