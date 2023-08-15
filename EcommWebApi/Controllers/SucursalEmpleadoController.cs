using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalEmpleadoController : ControllerBase
    {
        private readonly ILogger<SucursalEmpleadoController> _logger;
        private readonly ISucursalEmpleadoRepositorio _sucursalEmpleadoRepositorio;
        private readonly ISucursalRepositorio _sucursalRepositorio;
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        protected ApiResponse _response;

        public SucursalEmpleadoController(ILogger<SucursalEmpleadoController> logger
            , ISucursalEmpleadoRepositorio sucursalEmpleadoRepositorio
            , ISucursalRepositorio sucursalRepositorio
            , IEmpleadoRepositorio empleadoRepositorio)
        {
            _logger = logger;
            _sucursalEmpleadoRepositorio = sucursalEmpleadoRepositorio;
            _sucursalRepositorio = sucursalRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
            _response = new();
        }
        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateSucursal([FromBody] SucursalEmpleadoCreateDto createDto)
        {
            try
            {
                if (createDto is null)
                    return BadRequest();

                if (createDto.EmpleadoId <= 0
                    || createDto.SucursalId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion",
                        "Es necesario llenar a los campos que pertenecen a la sucursal"
                    };
                    return BadRequest(_response);
                }

                var empleado = await _empleadoRepositorio.Obtener(emp => emp.IdEmpleado == createDto.EmpleadoId,
                    incluir: query => query
                    .Include(emp => emp.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(emp => emp.Rol));
                var surcursal = await _sucursalRepositorio.Obtener(s => s.IdSucursal == createDto.SucursalId,
                    incluir: query => query
                    .Include(s => s.Cadena)
                        .ThenInclude(c => c.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                    .Include(s => s.Cadena)
                        .ThenInclude(c => c.TipoCadena)
                            .ThenInclude(tc => tc.ModeloNegocio)
                    .Include(s => s.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(es => es.Pais)
                    .Include(s => s.EstadoSucursal)
                    .Include(s => s.ServiciosSucursal));

                SucursalEmpleado modelo = new()
                {
                    FechaIngreso = createDto.FechaIngreso,
                    Empleado = empleado,
                    Sucursal = surcursal
                };

                await _sucursalEmpleadoRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetSucursalEmpleado", new { id = modelo.IdSucursalEmpleado }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetSucursalEmpleados()
        {
            IEnumerable<SucursalEmpleado> sucursalEmpleadoList = await _sucursalEmpleadoRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(se => se.Empleado)
                    .ThenInclude(em => em.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(es => es.Pais)
                .Include(se => se.Empleado)
                    .ThenInclude(em => em.Rol)
                .Include(se => se.Sucursal)
                    .ThenInclude(s => s.Cadena)
                        .ThenInclude(c => c.TipoCadena)
                            .ThenInclude(tc => tc.ModeloNegocio)
                .Include(se => se.Sucursal)
                    .ThenInclude(em => em.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(es => es.Pais)
                .Include(se => se.Sucursal)
                    .ThenInclude(s => s.EstadoSucursal)
                .Include(se => se.Sucursal)
                    .ThenInclude(s => s.ServiciosSucursal));

            _response.Resultado = sucursalEmpleadoList.Select(s => new SucursalEmpleadoGetDto
            {
                IdSucursalEmpleado = s.IdSucursalEmpleado,
                FechaIngreso = s.FechaIngreso,
                Empleado = s.Empleado,
                Sucursal = s.Sucursal
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetSucursalEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetSucursalEmpleado(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al datos del la sucursal con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var sucursalEmpleado = await _sucursalEmpleadoRepositorio.Obtener(
                    x => x.IdSucursalEmpleado == id,
                    incluir: query => query
                    .Include(se => se.Empleado)
                        .ThenInclude(em => em.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                    .Include(se => se.Empleado)
                        .ThenInclude(em => em.Rol)
                    .Include(se => se.Sucursal)
                        .ThenInclude(s => s.Cadena)
                            .ThenInclude(c => c.TipoCadena)
                                .ThenInclude(tc => tc.ModeloNegocio)
                    .Include(se => se.Sucursal)
                        .ThenInclude(em => em.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                    .Include(se => se.Sucursal)
                        .ThenInclude(s => s.EstadoSucursal)
                    .Include(se => se.Sucursal)
                        .ThenInclude(s => s.ServiciosSucursal));

                if (sucursalEmpleado == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new SucursalEmpleadoGetDto
                {
                    IdSucursalEmpleado = sucursalEmpleado.IdSucursalEmpleado,
                    FechaIngreso = sucursalEmpleado.FechaIngreso,
                    Empleado = sucursalEmpleado.Empleado,
                    Sucursal = sucursalEmpleado.Sucursal
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

        [HttpGet("GetPorNombreSucursalOEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetPorNombreSucursalOEmpleado([FromQuery] string nombre)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>() { "El parámetro 'nombre' es requerido." };
                    return BadRequest(_response);
                }

                IEnumerable<SucursalEmpleado> sucursalEmpleados = await _sucursalEmpleadoRepositorio.ObtenerTodos(
                    s => s.Sucursal.NombreSucursal.Contains(nombre)
                    || s.Empleado.NombreEmpleado.Contains(nombre),
                    incluir: query => query
                    .Include(se => se.Empleado)
                        .ThenInclude(em => em.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                    .Include(se => se.Empleado)
                        .ThenInclude(em => em.Rol)
                    .Include(se => se.Sucursal)
                        .ThenInclude(s => s.Cadena)
                            .ThenInclude(c => c.TipoCadena)
                                .ThenInclude(tc => tc.ModeloNegocio)
                    .Include(se => se.Sucursal)
                        .ThenInclude(em => em.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                    .Include(se => se.Sucursal)
                        .ThenInclude(s => s.EstadoSucursal)
                    .Include(se => se.Sucursal)
                        .ThenInclude(s => s.ServiciosSucursal));

                if (sucursalEmpleados == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = sucursalEmpleados;
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
        public async Task<IActionResult> UpdateSucursalEmpleado(int id, [FromBody] SucursalEmpleadoUpdateDto updateDto)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.EmpleadoId <= 0
                    || updateDto.SucursalId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar la sucursal",
                        "Es necesario hacer referencia a los campos de la sucursal"
                    };
                    return BadRequest(_response);
                }

                var sucursalEmpleadoExistente = await _sucursalEmpleadoRepositorio.Obtener(s => s.IdSucursalEmpleado == id);
                if (sucursalEmpleadoExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                sucursalEmpleadoExistente.EmpleadoId = updateDto.EmpleadoId;
                sucursalEmpleadoExistente.SucursalId = updateDto.SucursalId;

                await _sucursalEmpleadoRepositorio.Actualizar(sucursalEmpleadoExistente);
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
        public async Task<IActionResult> DeleteSucursalEmpleado(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var sucursalEmpleado = await _sucursalEmpleadoRepositorio.Obtener(x => x.IdSucursalEmpleado == id);
                if (sucursalEmpleado == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _sucursalEmpleadoRepositorio.Remover(sucursalEmpleado);
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
