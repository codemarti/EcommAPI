namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface ISubproductoRepositorio : IRepositorio<Subproducto>
    {
        Task<Subproducto> Actualizar(Subproducto entidad);
    }
}
