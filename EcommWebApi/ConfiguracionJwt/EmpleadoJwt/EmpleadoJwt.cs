namespace EcommWebApi.ConfiguracionJwt.EmpleadoJwt
{
    public class EmpleadoJwt : ControllerBase, IEmpleadoJwt
    {
        private readonly IEmpleadoRepositorio _empleado;
        private readonly IConfiguration _configuracion;

        public EmpleadoJwt(IEmpleadoRepositorio empleado, IConfiguration configuracion)
        {
            _empleado = empleado;
            _configuracion = configuracion;
        }

        public async Task<Empleado> Autenticacion(LoginData loginEmpleado)
        {
            var empleadoActual = await _empleado.Obtener(u => u.NickEmpleado == loginEmpleado.Username || u.EmailEmpleado == loginEmpleado.Email,
                incluir: query => query
                .Include(e => e.Rol));

            if (empleadoActual == null || !BCrypt.Net.BCrypt.Verify(loginEmpleado.Password, empleadoActual.PassEmpleado))
                throw new Exception("Usuario no encontrado.");

            return empleadoActual;
        }
        public string GenerarToken(Empleado empleado)
        {
            var llaveSegura = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["JwtConfiguracion:Key"]));
            var credenciales = new SigningCredentials(llaveSegura, SecurityAlgorithms.HmacSha256);

            #region " RECLAMACIONES "
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, empleado.EmailEmpleado),
                new Claim(ClaimTypes.NameIdentifier, empleado.NickEmpleado)
            };

            // Si el empleado tiene un rol, se incluye en las reclamaciones
            if (empleado.Rol != null && !string.IsNullOrEmpty(empleado.Rol.NombreRol))
            {
                claims.Add(new Claim(ClaimTypes.Role, empleado.Rol.NombreRol));
            }
            #endregion
            #region " CREACION DEL TOKEN "
            var token = new JwtSecurityToken(
                _configuracion["JwtConfiguracion:Issuer"],
                _configuracion["JwtConfiguracion:Audience"],
                claims,
                expires: DateTime.Now.AddMonths(2),
                signingCredentials: credenciales
            );
            #endregion

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
