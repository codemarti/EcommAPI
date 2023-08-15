namespace EcommWebApi.ConfiguracionJwt.UsuarioJwt
{
    public class UsuarioJwt : ControllerBase, IUsuarioJwt
    {
        private readonly IUsuarioRepositorio _usuario;
        private readonly IConfiguration _configuracion;

        public UsuarioJwt(IUsuarioRepositorio usuario, IConfiguration configuracion)
        {
            _usuario = usuario;
            _configuracion = configuracion;
        }

        public async Task<Usuario> Autenticacion(LoginData loginUsuario)
        {
            var usuarioActual = await _usuario.Obtener(u => u.Nickname == loginUsuario.Username || u.EmailUsuario == loginUsuario.Email);

            if (usuarioActual == null || !BCrypt.Net.BCrypt.Verify(loginUsuario.Password, usuarioActual.PasswordUsuario))
                usuarioActual = null;

            return usuarioActual;
        }

        public string GenerarToken(Usuario usuario)
        {
            var llaveSegura = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["JwtConfiguracion:Key"]));
            var credenciales = new SigningCredentials(llaveSegura, SecurityAlgorithms.HmacSha256);

            #region " RECLAMACIONES "
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, usuario.EmailUsuario)
            };
            #endregion
            #region " CREACION DEL TOKEN "
            var token = new JwtSecurityToken(
                    _configuracion["JwtConfiguracion:Issuer"],
                    _configuracion["JwtConfiguracion:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(2),
                    signingCredentials: credenciales
                );
            #endregion

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
