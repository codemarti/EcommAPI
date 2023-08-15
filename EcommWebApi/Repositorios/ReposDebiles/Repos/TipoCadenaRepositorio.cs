namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class TipoCadenaRepositorio : Repositorio<TipoCadena>, ITipoCadenaRepositorio
    {
        private readonly DatosDbContext _context;

        public TipoCadenaRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TipoCadena> Actualizar(TipoCadena entidad)
        {
            _context.TipoCadenas.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
