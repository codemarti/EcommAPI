namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface ICategoriaRepositorio : IRepositorio<Categoria>
    {
        Task<Categoria> Actualizar(Categoria entidad);
    }
}
