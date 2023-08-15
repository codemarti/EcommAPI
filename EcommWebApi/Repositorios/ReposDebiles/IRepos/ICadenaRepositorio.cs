namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface ICadenaRepositorio : IRepositorio<Cadena>
    {
        Task<Cadena> Actualizar(Cadena entidad);
    }
}
