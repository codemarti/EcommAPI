namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface ISubproductoSerieRepositorio : IRepositorio<SubproductoSerie>
    {
        Task<SubproductoSerie> Actualizar(SubproductoSerie entidad);
    }
}
