namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IRolRepositorio : IRepositorio<Rol>
    {
        Task<Rol> Actualizar(Rol entidad);
    }
}
