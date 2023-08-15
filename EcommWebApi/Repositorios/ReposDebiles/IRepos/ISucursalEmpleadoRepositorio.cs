namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface ISucursalEmpleadoRepositorio : IRepositorio<SucursalEmpleado>
    {
        Task<SucursalEmpleado> Actualizar(SucursalEmpleado entidad);
    }
}
