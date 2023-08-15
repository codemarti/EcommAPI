using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers.HistorialControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialProductoController : ControllerBase
    {
        private readonly ILogger<HistorialProductoController> _logger;
        private readonly IRepositorio<HistorialProducto> _historialProductoRepositorio;
        protected ApiResponse _response;
        public HistorialProductoController(ILogger<HistorialProductoController> logger
            , IRepositorio<HistorialProducto> historialProductoRepositorio)
        {
            _logger = logger;
            _historialProductoRepositorio = historialProductoRepositorio;
            _response = new();
        }

        #region " APARTADO READ "
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetHistorialProductos()
        {
            _logger.LogInformation("Obtener todo el historial de productos");

            IEnumerable<HistorialProducto> historialProductoList = await _historialProductoRepositorio.ObtenerTodos();

            _response.Resultado = historialProductoList.Select(p => new HistorialProductoGetDto
            {
                IdHistorialProducto = p.IdHistorialProducto,
                FechaRealizado = p.FechaRealizado,
                NombreAccion = p.NombreAccion,
                NombreEmpleado = p.NombreEmpleado,
                NombreCategoriaAntigua = p.NombreCategoriaAntigua,
                NombreCategoriaNueva = p.NombreCategoriaNueva,
                NombreMarcaAntigua = p.NombreMarcaAntigua,
                NombreMarcaNueva = p.NombreMarcaNueva,
                NombreProductoAntiguo = p.NombreProductoAntiguo,
                NombreProductoNuevo = p.NombreProductoNuevo,
                SubproductoAfectado = p.SubproductoAfectado
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetHistorialProducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetHistorialProducto(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener historial de producto con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var historialProducto = await _historialProductoRepositorio.Obtener(x => x.IdHistorialProducto == id);
                if (historialProducto == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new HistorialProductoGetDto
                {
                    IdHistorialProducto = historialProducto.IdHistorialProducto,
                    FechaRealizado = historialProducto.FechaRealizado,
                    NombreAccion = historialProducto.NombreAccion,
                    NombreEmpleado = historialProducto.NombreEmpleado,
                    NombreCategoriaAntigua = historialProducto.NombreCategoriaAntigua,
                    NombreCategoriaNueva = historialProducto.NombreCategoriaNueva,
                    NombreMarcaAntigua = historialProducto.NombreMarcaAntigua,
                    NombreMarcaNueva = historialProducto.NombreMarcaNueva,
                    NombreProductoAntiguo = historialProducto.NombreProductoAntiguo,
                    NombreProductoNuevo = historialProducto.NombreProductoNuevo,
                    SubproductoAfectado = historialProducto.SubproductoAfectado
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

        [HttpGet("GetPorDataHistorialProducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> BuscarPorDataHistorialProducto([FromQuery] string data)
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

                // Obtener los productos por nombre
                IEnumerable<HistorialProducto> historialProductos = await _historialProductoRepositorio.ObtenerTodos(
                    p => p.NombreAccion.Contains(data)
                    || p.NombreEmpleado.Contains(data)
                    || p.NombreProductoAntiguo.Contains(data)
                    || p.NombreProductoNuevo.Contains(data)
                    || p.SubproductoAfectado.Contains(data));

                if (historialProductos == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = historialProductos;
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

        [HttpGet("GetPorRegistroHistorialProducto", Name = "GetPaginadoHistorialProducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetPaginado([FromQuery] int? pagina, int registros = 1)
        {
            try
            {
                if (registros == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>()
                    {
                        "Error de peticion",
                        "No se pueden mostrar 0 registros"
                    };
                    return StatusCode(StatusCodes.Status400BadRequest, _response);
                }
                int _pagina = pagina ?? 1;

                // Obtener los productos con las referencias de categorías y marcas cargadas
                IEnumerable<HistorialProducto> historialProductoList = await _historialProductoRepositorio
                    .ObtenerTodos();

                // Creo mi total de registros dependiente de la línea anterior
                decimal totalRegistros = historialProductoList.Count();

                int totalPaginas = Convert.ToInt32(Math.Ceiling(totalRegistros / registros));

                var productos = historialProductoList.Skip((_pagina - 1) * registros).Take(registros).ToList();

                _response.Resultado = new
                {
                    paginas = totalPaginas,
                    registros = productos,
                    currentPagina = _pagina
                };

                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
        #endregion
    }
}
