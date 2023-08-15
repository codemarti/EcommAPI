namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class EstadoCarritoRepositorio : Repositorio<EstadoCarrito>, IEstadoCarritoRepositorio
    {
        private readonly DatosDbContext _context;

        public EstadoCarritoRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EstadoCarrito> Actualizar(EstadoCarrito entidad)
        {
            _context.EstadoCarritos.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
