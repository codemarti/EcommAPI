using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers.HistorialControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialCategoriaController : ControllerBase
    {
        private readonly ILogger<HistorialCategoriaController> _logger;
        private readonly IRepositorio<HistorialCategoria> _historialCategoriaRepositorio;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public HistorialCategoriaController(ILogger<HistorialCategoriaController> logger
            , IRepositorio<HistorialCategoria> historialCategoriaRepositorio
            , IMapper mapper)
        {
            _logger = logger;
            _historialCategoriaRepositorio = historialCategoriaRepositorio;
            _mapper = mapper;
            _response = new();
        }

        #region " APARTADO READ "
        [HttpGet]
        //[Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse>> GetHistorialCategorias()
        {
            try
            {
                _logger.LogInformation("Obtener todo el historial de categorias");

                IEnumerable<HistorialCategoria> historialCategoriaList = await _historialCategoriaRepositorio.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<HistorialCategoriaGetDto>>(historialCategoriaList);
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

        [HttpGet("{id:int}", Name = "GetHistorialCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetHistorialCategoria(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error el historial de categoria con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var historialCategoria = await _historialCategoriaRepositorio.Obtener(x => x.IdHistorialCategoria == id);
                if (historialCategoria == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<CategoriaGetDto>(historialCategoria);
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

        [HttpGet("BuscarPorDataCategoriaHistorial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> BuscarPorDataCategoriaHistorial([FromQuery] string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>() { "El parámetro 'nombre' es requerido." };
                    return BadRequest(_response);
                }

                var historialCategoria = await _historialCategoriaRepositorio.ObtenerTodos(
                    x => x.NombreAccion.Contains(data)
                    || x.NombreCategoriaAntigua.Contains(data)
                    || x.NombreCategoriaNueva.Contains(data)
                    || x.NombreEmpleado.Contains(data)
                    || x.ProductoAfectado.Contains(data));
                if (historialCategoria == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = historialCategoria;
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
