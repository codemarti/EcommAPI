namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface ISucursalRepositorio : IRepositorio<Sucursal>
    {
        Task<Sucursal> Actualizar(Sucursal entidad);
    }
}
