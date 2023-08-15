namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface ITipoCadenaRepositorio : IRepositorio<TipoCadena>
    {
        Task<TipoCadena> Actualizar(TipoCadena entidad);
    }
}
