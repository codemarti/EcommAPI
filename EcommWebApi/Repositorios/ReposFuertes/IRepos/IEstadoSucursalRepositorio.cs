namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IEstadoSucursalRepositorio : IRepositorio<EstadoSucursal>
    {
        Task<EstadoSucursal> Actualizar(EstadoSucursal entidad);
    }
}
