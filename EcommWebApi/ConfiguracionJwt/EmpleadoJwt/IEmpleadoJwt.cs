namespace EcommWebApi.ConfiguracionJwt.EmpleadoJwt
{
    public interface IEmpleadoJwt
    {
        Task<Empleado> Autenticacion(LoginData loginEmpleado);
        string GenerarToken(Empleado empleado);
    }
}
