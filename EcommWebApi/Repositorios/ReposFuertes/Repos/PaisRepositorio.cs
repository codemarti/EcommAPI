namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class PaisRepositorio : Repositorio<Pais>, IPaisRepositorio
    {
        private readonly DatosDbContext _context;

        public PaisRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Pais> Actualizar(Pais entidad)
        {
            _context.Paises.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
