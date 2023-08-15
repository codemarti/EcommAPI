namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface IDireccionRepositorio : IRepositorio<Direccion>
    {
        Task<Direccion> Actualizar(Direccion entidad);
    }
}
