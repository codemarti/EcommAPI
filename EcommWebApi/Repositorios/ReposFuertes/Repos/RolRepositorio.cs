namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class RolRepositorio : Repositorio<Rol>, IRolRepositorio
    {
        private readonly DatosDbContext _context;

        public RolRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Rol> Actualizar(Rol entidad)
        {
            _context.Roles.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
