namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IModeloNegocioRepositorio : IRepositorio<ModeloNegocio>
    {
        Task<ModeloNegocio> Actualizar(ModeloNegocio entidad);
    }
}
