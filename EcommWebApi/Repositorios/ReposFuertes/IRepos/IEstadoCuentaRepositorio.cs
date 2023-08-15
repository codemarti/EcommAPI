namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IEstadoCuentaRepositorio : IRepositorio<EstadoCuenta>
    {
        Task<EstadoCuenta> Actualizar(EstadoCuenta entidad);
    }
}
