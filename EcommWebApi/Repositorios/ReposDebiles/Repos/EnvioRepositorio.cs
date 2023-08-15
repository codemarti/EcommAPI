namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class EnvioRepositorio : Repositorio<Envio>, IEnvioRepositorio
    {
        private readonly DatosDbContext _context;

        public EnvioRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Envio> Actualizar(Envio entidad)
        {
            _context.Envios.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
