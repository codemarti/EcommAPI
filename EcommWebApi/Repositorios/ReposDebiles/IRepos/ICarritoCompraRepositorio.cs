namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface ICarritoCompraRepositorio : IRepositorio<CarritoCompra>
    {
        Task<CarritoCompra> Actualizar(CarritoCompra entidad);
    }
}
