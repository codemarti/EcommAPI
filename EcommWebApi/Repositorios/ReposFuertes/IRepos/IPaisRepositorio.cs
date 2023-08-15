namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IPaisRepositorio : IRepositorio<Pais>
    {
        Task<Pais> Actualizar(Pais entidad);
    }
}
