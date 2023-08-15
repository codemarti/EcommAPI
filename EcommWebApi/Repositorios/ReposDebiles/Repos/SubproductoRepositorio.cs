namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class SubproductoRepositorio : Repositorio<Subproducto>, ISubproductoRepositorio
    {
        private readonly DatosDbContext _context;

        public SubproductoRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Subproducto> Actualizar(Subproducto entidad)
        {
            entidad.FechaActualizacion = DateTime.UtcNow;
            _context.Subproductos.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
