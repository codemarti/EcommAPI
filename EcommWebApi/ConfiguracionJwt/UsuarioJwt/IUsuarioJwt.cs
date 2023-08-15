namespace EcommWebApi.ConfiguracionJwt.UsuarioJwt
{
    public interface IUsuarioJwt
    {
        Task<Usuario> Autenticacion(LoginData loginUsuario);
        string GenerarToken(Usuario usuario);
        //Usuario GetUsuarioActual();
    }
}
