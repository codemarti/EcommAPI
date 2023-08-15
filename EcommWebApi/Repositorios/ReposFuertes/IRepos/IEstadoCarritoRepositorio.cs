namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IEstadoCarritoRepositorio : IRepositorio<EstadoCarrito>
    {
        Task<EstadoCarrito> Actualizar(EstadoCarrito entidad);
    }
}
