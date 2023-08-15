namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class EstadoPagoRepositorio : Repositorio<EstadoPago>, IEstadoPagoRepositorio
    {
        private readonly DatosDbContext _context;

        public EstadoPagoRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EstadoPago> Actualizar(EstadoPago entidad)
        {
            _context.EstadoPagos.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
