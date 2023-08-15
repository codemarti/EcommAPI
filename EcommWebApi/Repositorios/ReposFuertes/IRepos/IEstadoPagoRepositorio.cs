namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IEstadoPagoRepositorio : IRepositorio<EstadoPago>
    {
        Task<EstadoPago> Actualizar(EstadoPago entidad);
    }
}
