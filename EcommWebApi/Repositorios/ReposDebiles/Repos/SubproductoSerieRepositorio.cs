namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class SubproductoSerieRepositorio : Repositorio<SubproductoSerie>, ISubproductoSerieRepositorio
    {
        private readonly DatosDbContext _context;

        public SubproductoSerieRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SubproductoSerie> Actualizar(SubproductoSerie entidad)
        {
            _context.SubproductosSerie.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
