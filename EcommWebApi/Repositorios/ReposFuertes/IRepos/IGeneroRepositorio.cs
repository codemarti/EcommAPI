namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IGeneroRepositorio : IRepositorio<Genero>
    {
        Task<Genero> Actualizar(Genero entidad);
    }
}
