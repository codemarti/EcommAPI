namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface IEnvioRepositorio : IRepositorio<Envio>
    {
        Task<Envio> Actualizar(Envio entidad);
    }
}
