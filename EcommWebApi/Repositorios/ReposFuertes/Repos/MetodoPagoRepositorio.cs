namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class MetodoPagoRepositorio : Repositorio<MetodoPago>, IMetodoPagoRepositorio
    {
        private readonly DatosDbContext _context;

        public MetodoPagoRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<MetodoPago> Actualizar(MetodoPago entidad)
        {
            _context.MetodoPagos.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
