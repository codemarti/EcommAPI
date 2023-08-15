namespace EcommWebApi.Repositorios.ReposFuertes.IRepos
{
    public interface IHorarioSucursalRepositorio : IRepositorio<HorarioSucursal>
    {
        Task<HorarioSucursal> Actualizar(HorarioSucursal entidad);
    }
}
