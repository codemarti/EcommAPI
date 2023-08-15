namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface ICarritoProductoRepositorio : IRepositorio<CarritoProducto>
    {
        Task<CarritoProducto> Actualizar(CarritoProducto entidad);
    }
}
