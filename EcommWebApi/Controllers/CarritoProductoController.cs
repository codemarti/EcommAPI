using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoProductoController : ControllerBase
    {
        private readonly ILogger<CarritoProductoController> _logger;
        private readonly ICarritoProductoRepositorio _carritoProductoRepositorio;
        private readonly ISubproductoRepositorio _subproductoRepositorio;
        protected ApiResponse _response;

        public CarritoProductoController(ILogger<CarritoProductoController> logger
            , ICarritoProductoRepositorio carritoProductoRepositorio
            , ISubproductoRepositorio subproductoRepositorio)
        {
            _logger = logger;
            _carritoProductoRepositorio = carritoProductoRepositorio;
            _subproductoRepositorio = subproductoRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateCarritoProducto([FromBody] CarritoProductoCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (createDto is null)
                    return BadRequest();

                if (createDto.SubproductoId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al meter los productos en el carrito",
                        "Es necesario hacer referencia al carrito o los porductos que iran en el carrito de compras"
                    };
                    return BadRequest(_response);
                }

                var subproducto = await _subproductoRepositorio.Obtener(m => m.IdSubproducto == createDto.SubproductoId);

                // Crear el objeto Producto y asignar los valores correspondientes
                CarritoProducto modelo = new()
                {
                    Cantidad = createDto.Cantidad,
                    Subtotal = subproducto.PrecioSub * createDto.Cantidad,
                    FechaCreacionCarrito = DateTime.UtcNow,
                    FechaActualizacionCarrito = DateTime.UtcNow,
                    SubproductoId = createDto.SubproductoId
                };

                await _carritoProductoRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetCarritoProducto", new { id = modelo.IdCarritoProducto }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetCarritoPoductos()
        {
            IEnumerable<CarritoProducto> productoCaritoList = await _carritoProductoRepositorio.ObtenerTodos(
                incluir: query => query
                    .Include(cp => cp.Subproducto)
                        .ThenInclude(s=>s.Producto)
                            .ThenInclude(p => p.Categoria)
                    .Include(cp=>cp.Subproducto)
                        .ThenInclude(cp => cp.Producto)
                            .ThenInclude(p => p.Marca)
            );

            _response.Resultado = productoCaritoList.Select(p => new CarritoProductoGetDto
            {
                IdCarritoProducto = p.IdCarritoProducto,
                Cantidad = p.Cantidad,
                Subtotal = p.Subtotal,
                FechaActualizacionCarrito = p.FechaActualizacionCarrito,
                FechaCreacionCarrito = p.FechaCreacionCarrito,
                Subproducto = p.Subproducto
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetCarritoProducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetCarritoProducto(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener carrito y productos con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var carritoProducto = await _carritoProductoRepositorio.Obtener(
                    x => x.IdCarritoProducto == id,
                    incluir: query => query
                        .Include(cp => cp.Subproducto)
                            .ThenInclude(s => s.Producto)
                                .ThenInclude(p => p.Categoria)
                        .Include(cp => cp.Subproducto)
                            .ThenInclude(cp => cp.Producto)
                                .ThenInclude(p => p.Marca)
                );

                if (carritoProducto == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new CarritoProductoGetDto
                {
                    IdCarritoProducto = carritoProducto.IdCarritoProducto,
                    Cantidad = carritoProducto.Cantidad,
                    Subtotal = carritoProducto.Subtotal,
                    FechaActualizacionCarrito = carritoProducto.FechaActualizacionCarrito,
                    FechaCreacionCarrito = carritoProducto.FechaCreacionCarrito,
                    Subproducto = carritoProducto.Subproducto
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

        [HttpGet("GetPorRegistrosCarritoProducto", Name = "GetPaginadoCarritoProducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetPaginadoCarritoProducto([FromQuery] int? pagina, int registros = 1)
        {
            try
            {
                if (registros == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>()
                    {
                        "Error de petición",
                        "No se pueden mostrar 0 registros"
                    };
                    return BadRequest(_response);
                }

                int _pagina = pagina ?? 1;

                // Obtener los productos con las referencias de categorías y marcas cargadas
                IEnumerable<CarritoProducto> carritoProductoList = await _carritoProductoRepositorio.ObtenerTodos(
                incluir: query => query
                    .Include(cp => cp.Subproducto)
                        .ThenInclude(s => s.Producto)
                            .ThenInclude(p => p.Categoria)
                    .Include(cp => cp.Subproducto)
                        .ThenInclude(cp => cp.Producto)
                            .ThenInclude(p => p.Marca)
                );

                int totalRegistros = carritoProductoList.Count();
                int totalPaginas = (int)Math.Ceiling((decimal)totalRegistros / registros);
                var carritoProductos = carritoProductoList.Skip((_pagina - 1) * registros).Take(registros).ToList();

                _response.Resultado = new
                {
                    paginas = totalPaginas,
                    registros = carritoProductos,
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
        #region " APARTADO UPDATE "
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCarritoProducto(int id, [FromBody] CarritoProductoUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdCarritoProducto)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.SubproductoId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar los productos del carrito",
                        "Es necesario hacer referencia al carrito o los productos que iran en el carrito de compras"
                    };
                    return BadRequest(_response);
                }

                var carritoProductoExistente = await _carritoProductoRepositorio.Obtener(cp => cp.IdCarritoProducto == id);
                if (carritoProductoExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                var subproducto = await _subproductoRepositorio.Obtener(s => s.IdSubproducto == updateDto.SubproductoId);

                carritoProductoExistente.Cantidad = updateDto.Cantidad;
                carritoProductoExistente.SubproductoId = updateDto.SubproductoId;

                await _carritoProductoRepositorio.Actualizar(carritoProductoExistente);

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
        public async Task<IActionResult> DeleteCarritoProducto(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var carritoProduco = await _carritoProductoRepositorio.Obtener(x => x.IdCarritoProducto == id);
                if (carritoProduco == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _carritoProductoRepositorio.Remover(carritoProduco);
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
