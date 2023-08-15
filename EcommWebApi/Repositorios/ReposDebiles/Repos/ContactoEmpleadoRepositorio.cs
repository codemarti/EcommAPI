namespace EcommWebApi.Repositorios.ReposDebiles.Repos
{
    public class ContactoEmpleadoRepositorio : Repositorio<ContactoEmpleado>, IContactoEmpleadoRepositorio
    {
        private readonly DatosDbContext _context;

        public ContactoEmpleadoRepositorio(DatosDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ContactoEmpleado> Actualizar(ContactoEmpleado entidad)
        {
            _context.ContactoEmpleados.Update(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
