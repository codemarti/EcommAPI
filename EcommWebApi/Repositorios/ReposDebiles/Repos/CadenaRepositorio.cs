namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class CadenaRepositorio : Repositorio<Cadena>, ICadenaRepositorio
    {
        private readonly DatosDbContext _context;

        public CadenaRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cadena> Actualizar(Cadena entidad)
        {
            _context.Cadenas.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
