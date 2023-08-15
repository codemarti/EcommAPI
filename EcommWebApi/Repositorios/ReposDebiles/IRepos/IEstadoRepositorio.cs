namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface IEstadoRepositorio : IRepositorio<Estado>
    {
        Task<Estado> Actualizar(Estado entidad);
    }
}
