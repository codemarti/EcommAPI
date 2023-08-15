using Domain.Entidades.EHistorial;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers.HistorialControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialSubproductoController : ControllerBase
    {
        private readonly ILogger<HistorialSubproductoController> _logger;
        private readonly IRepositorio<HistorialSubproducto> _historialSubproductoRepositorio;
        protected ApiResponse _response;

        public HistorialSubproductoController(ILogger<HistorialSubproductoController> logger
            , IRepositorio<HistorialSubproducto> historialSubproductoRepositorio)
        {
            _logger = logger;
            _historialSubproductoRepositorio = historialSubproductoRepositorio;
            _response = new();
        }

        #region " APARTADO READ "
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetHistorialSubproductos()
        {
            IEnumerable<HistorialSubproducto> historialSubproductoList = await _historialSubproductoRepositorio.ObtenerTodos();

            _response.Resultado = historialSubproductoList.Select(hsp => new HistorialSubproductoGetDto
            {
                IdHistorialSubproducto = hsp.IdHistorialSubproducto,
                FechaRealizado = hsp.FechaRealizado,
                NombreAccion = hsp.NombreAccion,
                NombreEmpleado = hsp.NombreEmpleado,
                CodigoBarrasAntiguo = hsp.CodigoBarrasAntiguo,
                CodigoBarrasNuevo = hsp.CodigoBarrasNuevo,
                ImagenAntigua = hsp.ImagenAntigua,
                ImagenNueva = hsp.ImagenNueva,
                DescripcionAntigua = hsp.DescripcionAntigua,
                DescripcionNueva = hsp.DescripcionNueva,
                PrecioSubAntiguo = hsp.PrecioSubAntiguo,
                PrecioSubNuevo = hsp.PrecioSubNuevo,
                StockAntiguo = hsp.StockAntiguo,
                StockNuevo= hsp.StockNuevo,
                NombreProductoAntiguo=hsp.NombreProductoAntiguo,
                NombreProductoNuevo=hsp.NombreProductoNuevo,
                NombreSucursalAntigua=hsp.NombreSucursalAntigua,
                NombreSucursalNueva=hsp.NombreSucursalNueva,
                CarritoProductoAfectado=hsp.CarritoProductoAfectado,
                SubproductoSerieAfectado=hsp.SubproductoSerieAfectado
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetHistorialSubproducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetHistorialSubproductos(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener historial de subproductos con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }
                
                var historialSubproducto = await _historialSubproductoRepositorio.Obtener(x => x.IdHistorialSubproducto == id);

                if (historialSubproducto == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new HistorialSubproductoGetDto
                {
                    IdHistorialSubproducto = historialSubproducto.IdHistorialSubproducto,
                    FechaRealizado = historialSubproducto.FechaRealizado,
                    NombreAccion = historialSubproducto.NombreAccion,
                    NombreEmpleado = historialSubproducto.NombreEmpleado,
                    CodigoBarrasAntiguo = historialSubproducto.CodigoBarrasAntiguo,
                    CodigoBarrasNuevo = historialSubproducto.CodigoBarrasNuevo,
                    ImagenAntigua = historialSubproducto.ImagenAntigua,
                    ImagenNueva = historialSubproducto.ImagenNueva,
                    DescripcionAntigua = historialSubproducto.DescripcionAntigua,
                    DescripcionNueva = historialSubproducto.DescripcionNueva,
                    PrecioSubAntiguo = historialSubproducto.PrecioSubAntiguo,
                    PrecioSubNuevo = historialSubproducto.PrecioSubNuevo,
                    StockAntiguo = historialSubproducto.StockAntiguo,
                    StockNuevo = historialSubproducto.StockNuevo,
                    NombreProductoAntiguo = historialSubproducto.NombreProductoAntiguo,
                    NombreProductoNuevo = historialSubproducto.NombreProductoNuevo,
                    NombreSucursalAntigua = historialSubproducto.NombreSucursalAntigua,
                    NombreSucursalNueva = historialSubproducto.NombreSucursalNueva,
                    CarritoProductoAfectado = historialSubproducto.CarritoProductoAfectado,
                    SubproductoSerieAfectado = historialSubproducto.SubproductoSerieAfectado
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

        [HttpGet("GetHistorialSubproductosPorRegistros", Name = "GetPaginadoHistorialSubproductos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetPaginadoHistorialSubproductos(string dataSubproducto,
            [FromQuery] int? pagina,
            int registros = 1)
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
                IEnumerable<HistorialSubproducto> subproductoList = await _historialSubproductoRepositorio
                    .ObtenerTodos(hsp => hsp.CodigoBarrasAntiguo.Contains(dataSubproducto)
                    || hsp.NombreAccion.Contains(dataSubproducto)
                    || hsp.NombreEmpleado.Contains(dataSubproducto)
                    || hsp.FechaRealizado.Contains(dataSubproducto));

                // Creo mi total de registros dependiente de la línea anterior
                decimal totalRegistros = subproductoList.Count();
                int totalPaginas = Convert.ToInt32(Math.Ceiling(totalRegistros / registros));
                var productos = subproductoList.Skip((_pagina - 1) * registros).Take(registros).ToList();

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
