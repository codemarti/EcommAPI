namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IEstadoEmpleadoRepositorio : IRepositorio<EstadoEmpleado>
    {
        Task<EstadoEmpleado> Actualizar(EstadoEmpleado entidad);
    }
}
