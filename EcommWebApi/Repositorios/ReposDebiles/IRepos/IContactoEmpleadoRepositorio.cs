namespace EcommWebApi.Repositorios.ReposDebiles.IRepos
{
    public interface IContactoEmpleadoRepositorio : IRepositorio<ContactoEmpleado>
    {
        Task<ContactoEmpleado> Actualizar(ContactoEmpleado entidad);
    }
}
