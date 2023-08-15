using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoEmpleadoController : ControllerBase
    {
        private readonly ILogger<EstadoEmpleadoController> _logger;
        private readonly IEstadoEmpleadoRepositorio _edoEmpleadoRepositorio;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public EstadoEmpleadoController(ILogger<EstadoEmpleadoController> logger
            , IEstadoEmpleadoRepositorio edoEmpleadoRepositorio
            , IMapper mapper)
        {
            _logger = logger;
            _edoEmpleadoRepositorio = edoEmpleadoRepositorio;
            _mapper = mapper;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateEdoEmpleado([FromBody] EstadoEmpleadoCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (createDto is null)
                    return BadRequest();

                EstadoEmpleado edoEmpleado = _mapper.Map<EstadoEmpleado>(createDto);

                await _edoEmpleadoRepositorio.Crear(edoEmpleado);
                _response.Resultado = edoEmpleado;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetEdoEmpleado", new { id = edoEmpleado.IdEdoEmpleado }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetEdoEmpleados()
        {
            try
            {
                _logger.LogInformation("Obtener los horarios");

                IEnumerable<EstadoEmpleado> edoEmpleadoList = await _edoEmpleadoRepositorio.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<EstadoEmpleadoGetDto>>(edoEmpleadoList);
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

        [HttpGet("{id:int}", Name = "GetEdoEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetEdoEmpleado(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener estado de empleado con el ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var edoEmpleado = await _edoEmpleadoRepositorio.Obtener(x => x.IdEdoEmpleado == id);
                if (edoEmpleado == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<EstadoEmpleadoGetDto>(edoEmpleado);
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

        [HttpGet("BuscarPorNombreEdoEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> BuscarPorNombreEdoEmpleado([FromQuery] string nombre)
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

                var edoEmpleado = await _edoEmpleadoRepositorio.ObtenerTodos(x => x.NombreEdoEmpleado == nombre);
                if (edoEmpleado == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<EstadoEmpleadoGetDto>(edoEmpleado);
                _response.StatusCode = HttpStatusCode.OK;

                _response.Resultado = edoEmpleado;
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
        public async Task<IActionResult> UpdateEdoEmpleado(int id, [FromBody] EstadoEmpleadoUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdEdoEmpleado)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                EstadoEmpleado edoEmpleado = _mapper.Map<EstadoEmpleado>(updateDto);
                await _edoEmpleadoRepositorio.Actualizar(edoEmpleado);

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
        public async Task<IActionResult> DeleteEdoEmpleado(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var edoEmpleado = await _edoEmpleadoRepositorio.Obtener(x => x.IdEdoEmpleado == id);
                if (edoEmpleado == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _edoEmpleadoRepositorio.Remover(edoEmpleado);
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
