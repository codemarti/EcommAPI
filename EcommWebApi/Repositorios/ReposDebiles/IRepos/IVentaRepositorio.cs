namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface IVentaRepositorio : IRepositorio<Venta>
    {
        Task<Venta> Actualizar(Venta entidad);
    }
}
