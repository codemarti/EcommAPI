namespace EcommWebApi.Repositorios.ReposFuertes.Repos
{
    public class HorarioSucursalRepositorio : Repositorio<HorarioSucursal>, IHorarioSucursalRepositorio
    {
        private readonly DatosDbContext _context;

        public HorarioSucursalRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<HorarioSucursal> Actualizar(HorarioSucursal entidad)
        {
            _context.HorarioSucursales.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
