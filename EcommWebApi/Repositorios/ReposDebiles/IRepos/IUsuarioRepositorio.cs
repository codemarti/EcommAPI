namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface IUsuarioRepositorio : IRepositorio<Usuario>
    {
        Task<Usuario> Actualizar(Usuario entidad);
    }
}
