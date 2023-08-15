namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly DatosDbContext _context;

        public MarcaRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Marca> Actualizar(Marca entidad)
        {
            _context.Marcas.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
