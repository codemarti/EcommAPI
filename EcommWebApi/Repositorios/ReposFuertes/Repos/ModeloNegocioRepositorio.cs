namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class ModeloNegocioRepositorio : Repositorio<ModeloNegocio>, IModeloNegocioRepositorio
    {
        private readonly DatosDbContext _context;

        public ModeloNegocioRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ModeloNegocio> Actualizar(ModeloNegocio entidad)
        {
            _context.ModeloNegocios.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
