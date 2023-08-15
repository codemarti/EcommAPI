namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface ICiudadRepositorio : IRepositorio<Ciudad>
    {
        Task<Ciudad> Actualizar(Ciudad entidad);
    }
}
