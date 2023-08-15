using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IDireccionRepositorio _direccionRepositorio;
        private readonly IEstadoCuentaRepositorio _edoCuentaRepositorio;
        private readonly IGeneroRepositorio _generoRepositorio;
        private readonly IUsuarioJwt _usuarioJwt;
        protected ApiResponse _response;

        public UsuarioController(ILogger<UsuarioController> logger
            , IUsuarioRepositorio usuarioRepositorio
            , IDireccionRepositorio direccionRepositorio
            , IEstadoCuentaRepositorio edoCuentaRepositorio
            , IGeneroRepositorio generoRepositorio
            , IUsuarioJwt usuarioJwt)
        {
            _logger = logger;
            _usuarioRepositorio = usuarioRepositorio;
            _direccionRepositorio = direccionRepositorio;
            _edoCuentaRepositorio = edoCuentaRepositorio;
            _generoRepositorio = generoRepositorio;
            _usuarioJwt = usuarioJwt;
            _response = new();
        }

        #region " LOGIN "
        [HttpPost("LoginUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> LoginUsuario(LoginData loginUsuario)
        {
            try
            {
                if (loginUsuario == null)
                    return BadRequest();

                var usuario = await _usuarioJwt.Autenticacion(loginUsuario);
                if (usuario == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error",
                        "Es necesario ingresar tu usuario y contraseña"
                    };
                    return BadRequest(_response);
                }

                var tokenJwt = _usuarioJwt.GenerarToken(usuario);
                _response.Resultado = new
                {
                    token = tokenJwt,
                    mensaje = "Token generado exitosamente."
                };
                _response.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception ex)
            {
                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        #endregion

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateUsuario([FromBody] UsuarioCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _usuarioRepositorio.Obtener(u => u.TelUsuario == createDto.TelUsuario) != null)
                {
                    ModelState.AddModelError("telefonoExiste", "Error este telefono ya existe");
                    return BadRequest(ModelState);
                }

                if (await _usuarioRepositorio.Obtener(u => u.Nickname.ToLower() == createDto.Nickname.ToLower()) != null)
                {
                    ModelState.AddModelError("nombreUsarioExiste", "Error este nombre de usuario ya existe");
                    return BadRequest(ModelState);
                }

                if (await _usuarioRepositorio.Obtener(u => u.EmailUsuario.ToLower() == createDto.EmailUsuario.ToLower()) != null)
                {
                    ModelState.AddModelError("correoExiste", "Error este correo ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                if (createDto.DireccionId <= 0
                    || createDto.EdoCuentaId <= 0
                    || createDto.GeneroId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion",
                        "Es necesario llenar a los campos que pertenecen al usuario"
                    };
                    return BadRequest(_response);
                }

                var direccion = await _direccionRepositorio.Obtener(d => d.IdDireccion == createDto.DireccionId,
                    incluir: query => query
                    .Include(d => d.Ciudad)
                        .ThenInclude(ci => ci.Estado)
                            .ThenInclude(e => e.Pais));
                var edoCuenta = await _edoCuentaRepositorio.Obtener(sc => sc.IdEdoCuenta == createDto.EdoCuentaId);
                var genero = await _generoRepositorio.Obtener(es => es.IdGenero == createDto.GeneroId);

                Usuario modelo = new()
                {
                    NombreUsuario = createDto.NombreUsuario,
                    ApellidosUsuario = createDto.ApellidosUsuario,
                    TelUsuario = createDto.TelUsuario,
                    FechaNacUsuario = createDto.FechaNacUsuario,
                    Nickname = createDto.Nickname,
                    EmailUsuario = createDto.EmailUsuario,
                    PasswordUsuario = BCrypt.Net.BCrypt.HashPassword(createDto.PasswordUsuario),
                    FechaRegistro = DateTime.UtcNow,
                    Direccion = direccion,
                    EstadoCuenta = edoCuenta,
                    Genero = genero
                };

                await _usuarioRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetUsuario", new { id = modelo.IdUsuario }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        #endregion
        #region " APARTADO READ "
        [HttpGet]
        //[Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetUsuarios()
        {
            IEnumerable<Usuario> usuarioList = await _usuarioRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(u => u.Direccion)
                    .ThenInclude(d => d.Ciudad)
                        .ThenInclude(ci => ci.Estado)
                            .ThenInclude(e => e.Pais)
                .Include(u => u.EstadoCuenta)
                .Include(u => u.Genero));

            _response.Resultado = usuarioList.Select(u => new UsuarioGetDto
            {
                IdUsuario = u.IdUsuario,
                NombreUsuario = u.NombreUsuario,
                ApellidosUsuario = u.ApellidosUsuario,
                TelUsuario = u.TelUsuario,
                FechaNacUsuario = u.FechaNacUsuario,
                Nickname = u.Nickname,
                EmailUsuario = u.EmailUsuario,
                PasswordUsuario = u.PasswordUsuario,
                Direccion = u.Direccion,
                EstadoCuenta = u.EstadoCuenta,
                Genero = u.Genero
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetUsuario(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al usuario con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var usuario = await _usuarioRepositorio.Obtener(
                    x => x.IdUsuario == id,
                    incluir: query => query
                    .Include(u => u.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(u => u.EstadoCuenta)
                    .Include(u => u.Genero));

                if (usuario == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new UsuarioGetDto
                {
                    IdUsuario = id,
                    NombreUsuario = usuario.NombreUsuario,
                    ApellidosUsuario = usuario.ApellidosUsuario,
                    TelUsuario = usuario.TelUsuario,
                    FechaNacUsuario = usuario.FechaNacUsuario,
                    Nickname = usuario.Nickname,
                    EmailUsuario = usuario.EmailUsuario,
                    PasswordUsuario = usuario.PasswordUsuario,
                    Direccion = usuario.Direccion,
                    EstadoCuenta = usuario.EstadoCuenta,
                    Genero = usuario.Genero
                };

                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("GetPorDataUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetPorDataUsuario([FromQuery] string datosUsuario)
        {
            try
            {
                if (string.IsNullOrEmpty(datosUsuario))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>() { "El parámetro 'nombre' es requerido." };
                    return BadRequest(_response);
                }

                IEnumerable<Usuario> usuarios = await _usuarioRepositorio.ObtenerTodos(
                    u => (u.NombreUsuario + " " + u.ApellidosUsuario).Contains(datosUsuario)
                    || u.TelUsuario.Contains(datosUsuario),
                    incluir: query => query
                    .Include(u => u.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(u => u.EstadoCuenta)
                    .Include(u => u.Genero));

                _response.Resultado = usuarios;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("GetPorCuentaUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetPorCuentaUsuario([FromQuery] string cuentaUsuario)
        {
            try
            {
                if (string.IsNullOrEmpty(cuentaUsuario))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>() { "El parámetro 'correo' es requerido." };
                    return BadRequest(_response);
                }

                IEnumerable<Usuario> usuarios = await _usuarioRepositorio.ObtenerTodos(
                    u => u.Nickname.Contains(cuentaUsuario)
                    || u.EmailUsuario.Contains(cuentaUsuario),
                    incluir: query => query
                    .Include(u => u.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(u => u.EstadoCuenta)
                    .Include(u => u.Genero));

                _response.Resultado = usuarios;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
        #endregion
        #region " APARTADO UPDATE "
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UsuarioUpdateDto updateDto)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                if (updateDto.DireccionId <= 0
                    || updateDto.EdoCuentaId <= 0
                    || updateDto.GeneroId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar la usuario",
                        "Es necesario hacer referencia a los campos del usuario"
                    };
                    return BadRequest(_response);
                }

                var usuarioExistente = await _usuarioRepositorio.Obtener(s => s.IdUsuario == id);
                if (usuarioExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                usuarioExistente.NombreUsuario = updateDto.NombreUsuario;
                usuarioExistente.ApellidosUsuario = updateDto.ApellidosUsuario;
                usuarioExistente.TelUsuario = updateDto.TelUsuario;
                usuarioExistente.FechaNacUsuario = updateDto.FechaNacUsuario;
                usuarioExistente.Nickname = updateDto.Nickname;
                usuarioExistente.PasswordUsuario = updateDto.PasswordUsuario;
                usuarioExistente.DireccionId = updateDto.DireccionId;
                usuarioExistente.EdoCuentaId = updateDto.EdoCuentaId;
                usuarioExistente.GeneroId = updateDto.GeneroId;

                await _usuarioRepositorio.Actualizar(usuarioExistente);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }
        #endregion
        #region " APARTADO DELETE "
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var usuario = await _usuarioRepositorio.Obtener(x => x.IdUsuario == id);
                if (usuario == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _usuarioRepositorio.Remover(usuario);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }
        #endregion
    }
}
