namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IMetodoPagoRepositorio : IRepositorio<MetodoPago>
    {
        Task<MetodoPago> Actualizar(MetodoPago entidad);
    }
}
