namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class MetodoEnvioRepositorio : Repositorio<MetodoEnvio>, IMetodoEnvioRepositorio
    {
        private readonly DatosDbContext _context;

        public MetodoEnvioRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<MetodoEnvio> Actualizar(MetodoEnvio entidad)
        {
            _context.MetodoEnvios.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
