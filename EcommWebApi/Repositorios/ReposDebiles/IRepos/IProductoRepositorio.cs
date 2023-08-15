namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        Task<Producto> Actualizar(Producto entidad);
    }
}
