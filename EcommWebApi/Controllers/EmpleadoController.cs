using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly ILogger<EmpleadoController> _logger;
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        private readonly IDireccionRepositorio _direccionRepositorio;
        private readonly IEstadoEmpleadoRepositorio _edoEmpleadoRepositorio;
        private readonly IGeneroRepositorio _generoRepositorio;
        private readonly IRolRepositorio _rolRepositorio;
        protected ApiResponse _response;

        private readonly IEmpleadoJwt _empleadoJwt;

        public EmpleadoController(ILogger<EmpleadoController> logger
            , IEmpleadoRepositorio empleadoRepositorio
            , IDireccionRepositorio direccionRepositorio
            , IEstadoEmpleadoRepositorio edoEmpleadoRepositorio
            , IGeneroRepositorio generoRepositorio
            , IRolRepositorio rolRepositorio
            , IEmpleadoJwt empleadoJwt)
        {
            _logger = logger;
            _empleadoRepositorio = empleadoRepositorio;
            _direccionRepositorio = direccionRepositorio;
            _edoEmpleadoRepositorio = edoEmpleadoRepositorio;
            _generoRepositorio = generoRepositorio;
            _rolRepositorio = rolRepositorio;
            _empleadoJwt = empleadoJwt;
            _response = new();
        }

        #region " LOGIN "
        [HttpPost("LoginEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> LoginEmpleado(LoginData loginEmpleado)
        {
            try
            {
                if (loginEmpleado == null)
                    return BadRequest();

                var empleado = await _empleadoJwt.Autenticacion(loginEmpleado);
                if (empleado == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error",
                        "Es necesario ingresar tu empleado y contraseña"
                    };
                    return BadRequest(_response);
                }

                var tokenJwt = _empleadoJwt.GenerarToken(empleado);
                if (tokenJwt == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NoContent;
                    _response.ErrorMessages = new List<string> { "Hola" };
                }

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
                _response.ErrorMessages = new List<string> { ex.ToString(), "No existe usuario" };
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
        public async Task<ActionResult<ApiResponse>> CreateEmpleado([FromBody] EmpleadoCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (createDto is null)
                    return BadRequest();

                if (createDto.DireccionId <= 0 || createDto.RolId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Error de peticion al hacer referencia a una direccion" };
                    return BadRequest(_response);
                }

                var direccion = await _direccionRepositorio.Obtener(c => c.IdDireccion == createDto.DireccionId,
                    incluir: query => query
                    .Include(d=>d.Ciudad)
                        .ThenInclude(c => c.Estado)
                            .ThenInclude(e => e.Pais));
                var edoEmpleado = await _edoEmpleadoRepositorio.Obtener(ee => ee.IdEdoEmpleado == createDto.EdoEmpleadoId);
                var genero = await _generoRepositorio.Obtener(c => c.IdGenero == createDto.GeneroId);
                var rol = await _rolRepositorio.Obtener(c => c.IdRol == createDto.RolId);


                Empleado modelo = new()
                {
                    RFCE = createDto.RFCE,
                    NumEmpleado = createDto.NumEmpleado,
                    NombreEmpleado = createDto.NombreEmpleado,
                    ApellidoEmpleado = createDto.ApellidoEmpleado,
                    TelEmpleado = createDto.TelEmpleado,
                    FechaNacEmpleado = createDto.FechaNacEmpleado,
                    FechaIngreso = DateTime.Now,
                    NickEmpleado = createDto.NickEmpleado,
                    EmailEmpleado = createDto.EmailEmpleado,
                    PassEmpleado = BCrypt.Net.BCrypt.HashPassword(createDto.PassEmpleado),
                    Direccion = direccion,
                    EstadoEmpleado = edoEmpleado,
                    Genero = genero,
                    Rol = rol
                };

                await _empleadoRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetEmpleado", new { id = modelo.IdEmpleado }, _response);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetEmpleados()
        {
            IEnumerable<Empleado> empleadoList = await _empleadoRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(em => em.Direccion)
                    .ThenInclude(d => d.Ciudad)
                        .ThenInclude(ci => ci.Estado)
                            .ThenInclude(e => e.Pais)
                .Include(em => em.EstadoEmpleado)
                .Include(em => em.Genero)
                .Include(em => em.Rol)
            );

            _response.Resultado = empleadoList.Select(e => new EmpleadoGetDto
            {
                IdEmpleado = e.IdEmpleado,
                NumEmpleado = e.NumEmpleado,
                RFCE = e.RFCE,
                NombreEmpleado = e.NombreEmpleado,
                ApellidoEmpleado = e.ApellidoEmpleado,
                TelEmpleado = e.TelEmpleado,
                FechaNacEmpleado = e.FechaNacEmpleado,
                FechaIngreso = e.FechaIngreso,
                FechaBaja = e.FechaBaja,
                NickEmpleado = e.NickEmpleado,
                EmailEmpleado = e.EmailEmpleado,
                PassEmpleado = e.PassEmpleado,
                Direccion = e.Direccion,
                EstadoEmpleado = e.EstadoEmpleado,
                Genero = e.Genero,
                Rol = e.Rol
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetEmpleado(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener empleado con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var empleado = await _empleadoRepositorio.Obtener(
                    x => x.IdEmpleado == id,
                    incluir: query => query
                    .Include(em => em.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(em => em.EstadoEmpleado)
                    .Include(em => em.Genero)
                    .Include(em => em.Rol)
                );

                if (empleado == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new EmpleadoGetDto
                {
                    IdEmpleado = empleado.IdEmpleado,
                    NumEmpleado = empleado.NumEmpleado,
                    RFCE = empleado.RFCE,
                    NombreEmpleado = empleado.NombreEmpleado,
                    ApellidoEmpleado = empleado.ApellidoEmpleado,
                    TelEmpleado = empleado.TelEmpleado,
                    FechaNacEmpleado = empleado.FechaNacEmpleado,
                    NickEmpleado = empleado.NickEmpleado,
                    EmailEmpleado = empleado.EmailEmpleado,
                    PassEmpleado = empleado.PassEmpleado,
                    Direccion = empleado.Direccion,
                    EstadoEmpleado = empleado.EstadoEmpleado,
                    Genero = empleado.Genero,
                    Rol = empleado.Rol
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

        [HttpGet("GetPorDataEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetPorDataEmpleado([FromQuery] string datosEmpleado)
        {
            try
            {
                if (string.IsNullOrEmpty(datosEmpleado))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>() { "El parámetro 'numero' es requerido." };
                    return BadRequest(_response);
                }

                IEnumerable<Empleado> empleados = await _empleadoRepositorio.ObtenerTodos(
                    e => e.RFCE.Contains(datosEmpleado)
                    || e.NumEmpleado.Contains(datosEmpleado)
                    || (e.NombreEmpleado + " " + e.ApellidoEmpleado).Contains(datosEmpleado),
                    incluir: query => query
                    .Include(em => em.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(em => em.EstadoEmpleado)
                    .Include(em => em.Genero)
                    .Include(em => em.Rol));

                if (empleados == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = empleados;
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

        [HttpGet("GetPorDataCuentaEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetPorDataCuentaEmpleado([FromQuery] string cuentaEmpleado)
        {
            try
            {
                if (string.IsNullOrEmpty(cuentaEmpleado))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>() { "El parámetro 'nombre' es requerido." };
                    return BadRequest(_response);
                }

                IEnumerable<Empleado> empleados = await _empleadoRepositorio.ObtenerTodos(
                    e => e.NickEmpleado.Contains(cuentaEmpleado)
                    || e.EmailEmpleado.Contains(cuentaEmpleado),
                    incluir: query => query
                    .Include(em => em.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(em => em.EstadoEmpleado)
                    .Include(em => em.Genero)
                    .Include(em => em.Rol));

                _response.Resultado = empleados;
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
        public async Task<IActionResult> UpdateEmpleado(int id, [FromBody] EmpleadoUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdEmpleado)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.DireccionId <= 0 || updateDto.RolId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar la direccion",
                        "Es necesario hacer a los campos que contiene empleado"
                    };
                    return BadRequest(_response);
                }

                var empleadoExistente = await _empleadoRepositorio.Obtener(d => d.IdEmpleado == id);
                if (empleadoExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                empleadoExistente.RFCE = updateDto.RFCE;
                empleadoExistente.NombreEmpleado = updateDto.NombreEmpleado;
                empleadoExistente.ApellidoEmpleado = updateDto.ApellidoEmpleado;
                empleadoExistente.TelEmpleado = updateDto.TelEmpleado;
                empleadoExistente.FechaNacEmpleado = updateDto.FechaNacEmpleado;
                empleadoExistente.FechaIngreso = updateDto.FechaIngreso;
                empleadoExistente.FechaBaja = updateDto.FechaBaja;
                empleadoExistente.NickEmpleado = updateDto.NickEmpleado;
                empleadoExistente.PassEmpleado = updateDto.PassEmpleado;
                empleadoExistente.DireccionId = updateDto.DireccionId;
                empleadoExistente.RolId = updateDto.RolId;

                await _empleadoRepositorio.Actualizar(empleadoExistente);
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
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var empleado = await _empleadoRepositorio.Obtener(x => x.IdEmpleado == id);
                if (empleado == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _empleadoRepositorio.Remover(empleado);
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
