namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
    {
        private readonly DatosDbContext _context;

        public UsuarioRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario> Actualizar(Usuario entidad)
        {
            _context.Usuarios.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
