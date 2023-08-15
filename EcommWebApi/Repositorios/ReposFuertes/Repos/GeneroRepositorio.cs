namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class GeneroRepositorio : Repositorio<Genero>, IGeneroRepositorio
    {
        private readonly DatosDbContext _context;

        public GeneroRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Genero> Actualizar(Genero entidad)
        {
            _context.Generos.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
