namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IMetodoEnvioRepositorio : IRepositorio<MetodoEnvio>
    {
        Task<MetodoEnvio> Actualizar(MetodoEnvio entidad);
    }
}
