using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers.HistorialControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialEmpleadoController : ControllerBase
    {
        private readonly ILogger<HistorialEmpleadoController> _logger;
        private readonly IRepositorio<HistorialEmpleado> _historialEmpleadoRepositorio;
        protected ApiResponse _response;

        public HistorialEmpleadoController(ILogger<HistorialEmpleadoController> logger
            , IRepositorio<HistorialEmpleado> historialEmpleadoRepositorio)
        {
            _logger = logger;
            _historialEmpleadoRepositorio = historialEmpleadoRepositorio;
            _response = new();
        }

        #region " APARTADO READ "
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetHistorialEmpleados()
        {
            _logger.LogInformation("Obtener todo el historial de empleados");
            IEnumerable<HistorialEmpleado> historialEmpleadoList = await _historialEmpleadoRepositorio.ObtenerTodos();

            _response.Resultado = historialEmpleadoList.Select(he => new HistorialEmpleadoGetDto
            {
                IdHistorialEmpleado = he.IdHistorialEmpleado,
                FechaRealizado = he.FechaRealizado,
                NombreAccion = he.NombreAccion,
                NombrePersona = he.NombrePersona,
                ApellidoEmpleadoAntiguo = he.ApellidoEmpleadoAntiguo,
                ApellidoEmpleadoNuevo = he.ApellidoEmpleadoNuevo,
                NombreEmpleadoAntiguo = he.NombreEmpleadoAntiguo,
                NombreEmpleadoNuevo = he.NombreEmpleadoNuevo,
                NumeroEmpleadoAntiguo = he.NumeroEmpleadoAntiguo,
                NumeroEmpleadoNuevo = he.NumeroEmpleadoNuevo,
                RFCAntiguo = he.RFCAntiguo,
                RFCNuevo = he.RFCNuevo,
                FechaNacimientoAntigua = he.FechaNacimientoAntigua,
                FechaNacimientoNueva = he.FechaNacimientoNueva,
                TelefonoEmpleadoAntiguo = he.TelefonoEmpleadoAntiguo,
                TelefonoEmpleadoNuevo = he.TelefonoEmpleadoNuevo,
                EmailEmpleadoAntiguo = he.EmailEmpleadoAntiguo,
                EmailEmpleadoNuevo = he.EmailEmpleadoNuevo,
                NombreUsuarioAntiguo = he.NombreUsuarioAntiguo,
                NombreUsuarioNuevo = he.NombreUsuarioNuevo,
                PasswordEmpleadoAntigua = he.PasswordEmpleadoAntigua,
                PasswordEmpleadoNueva = he.PasswordEmpleadoNueva,
                FechaIngresoAntigua = he.FechaIngresoAntigua,
                FechaIngresoNueva = he.FechaIngresoNueva,
                FechaBajaAntigua = he.FechaBajaAntigua,
                FechaBajaNueva = he.FechaBajaNueva,
                DireccionEmpleadoAntigua = he.DireccionEmpleadoAntigua,
                DireccionEmpleadoNueva = he.DireccionEmpleadoNueva,
                NombreEdoEmpleadoAntiguo = he.NombreEdoEmpleadoAntiguo,
                NombreEdoEmpleadoNuevo = he.NombreEdoEmpleadoNuevo,
                NombreGeneroAntiguo = he.NombreGeneroAntiguo,
                NombreGeneroNuevo = he.NombreGeneroNuevo,
                NombreRolAntiguo = he.NombreRolAntiguo,
                NombreRolNuevo = he.NombreRolNuevo
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetHistorialEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetHistorialEmpleado(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener historial de empleado con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var historialEmpleado = await _historialEmpleadoRepositorio.Obtener(x => x.IdHistorialEmpleado == id);

                if (historialEmpleado == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new HistorialEmpleadoGetDto
                {
                    IdHistorialEmpleado = historialEmpleado.IdHistorialEmpleado,
                    FechaRealizado = historialEmpleado.FechaRealizado,
                    NombreAccion = historialEmpleado.NombreAccion,
                    NombrePersona = historialEmpleado.NombrePersona,
                    ApellidoEmpleadoAntiguo = historialEmpleado.ApellidoEmpleadoAntiguo,
                    ApellidoEmpleadoNuevo = historialEmpleado.ApellidoEmpleadoNuevo,
                    NombreEmpleadoAntiguo = historialEmpleado.NombreEmpleadoAntiguo,
                    NombreEmpleadoNuevo = historialEmpleado.NombreEmpleadoNuevo,
                    NumeroEmpleadoAntiguo = historialEmpleado.NumeroEmpleadoAntiguo,
                    NumeroEmpleadoNuevo = historialEmpleado.NumeroEmpleadoNuevo,
                    RFCAntiguo = historialEmpleado.RFCAntiguo,
                    RFCNuevo = historialEmpleado.RFCNuevo,
                    FechaNacimientoAntigua = historialEmpleado.FechaNacimientoAntigua,
                    FechaNacimientoNueva = historialEmpleado.FechaNacimientoNueva,
                    TelefonoEmpleadoAntiguo = historialEmpleado.TelefonoEmpleadoAntiguo,
                    TelefonoEmpleadoNuevo = historialEmpleado.TelefonoEmpleadoNuevo,
                    EmailEmpleadoAntiguo = historialEmpleado.EmailEmpleadoAntiguo,
                    EmailEmpleadoNuevo = historialEmpleado.EmailEmpleadoNuevo,
                    NombreUsuarioAntiguo = historialEmpleado.NombreUsuarioAntiguo,
                    NombreUsuarioNuevo = historialEmpleado.NombreUsuarioNuevo,
                    PasswordEmpleadoAntigua = historialEmpleado.PasswordEmpleadoAntigua,
                    PasswordEmpleadoNueva = historialEmpleado.PasswordEmpleadoNueva,
                    FechaIngresoAntigua = historialEmpleado.FechaIngresoAntigua,
                    FechaIngresoNueva = historialEmpleado.FechaIngresoNueva,
                    FechaBajaAntigua = historialEmpleado.FechaBajaAntigua,
                    FechaBajaNueva = historialEmpleado.FechaBajaNueva,
                    DireccionEmpleadoAntigua = historialEmpleado.DireccionEmpleadoAntigua,
                    DireccionEmpleadoNueva = historialEmpleado.DireccionEmpleadoNueva,
                    NombreEdoEmpleadoAntiguo = historialEmpleado.NombreEdoEmpleadoAntiguo,
                    NombreEdoEmpleadoNuevo = historialEmpleado.NombreEdoEmpleadoNuevo,
                    NombreGeneroAntiguo = historialEmpleado.NombreGeneroAntiguo,
                    NombreGeneroNuevo = historialEmpleado.NombreGeneroNuevo,
                    NombreRolAntiguo = historialEmpleado.NombreRolAntiguo,
                    NombreRolNuevo = historialEmpleado.NombreRolNuevo
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

        [HttpGet("GetPorDataHistorialEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetPorDataHistorialEmpleado([FromQuery] string datosHistorialEmpleado)
        {
            try
            {
                if (string.IsNullOrEmpty(datosHistorialEmpleado))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>() { "El parámetro 'data' es requerido." };
                    return BadRequest(_response);
                }

                IEnumerable<HistorialEmpleado> historialEmpleados = await _historialEmpleadoRepositorio.ObtenerTodos(
                    he => he.NombreAccion.Contains(datosHistorialEmpleado)
                    || he.NombrePersona.Contains(datosHistorialEmpleado)
                    || he.NombreEmpleadoAntiguo.Contains(datosHistorialEmpleado)
                    || he.NombreEmpleadoNuevo.Contains(datosHistorialEmpleado));

                if (historialEmpleados == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = historialEmpleados;
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
    }
}
