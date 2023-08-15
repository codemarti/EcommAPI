namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IServiciosSucursalRepositorio : IRepositorio<ServiciosSucursal>
    {
        Task<ServiciosSucursal> Actualizar(ServiciosSucursal entidad);
    }
}
