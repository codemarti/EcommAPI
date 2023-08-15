namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface IEmpleadoRepositorio : IRepositorio<Empleado>
    {
        Task<Empleado> Actualizar(Empleado entidad);
    }
}
