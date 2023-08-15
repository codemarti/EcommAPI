namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IMarcaRepositorio : IRepositorio<Marca>
    {
        Task<Marca> Actualizar(Marca entidad);
    }
}
