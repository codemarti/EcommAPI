using Domain.Entidades.EDebiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoCompraController : ControllerBase
    {
        private readonly ILogger<CarritoCompraController> _logger;
        private readonly ICarritoCompraRepositorio _carritoRepositorio;
        private readonly ICarritoProductoRepositorio _carritoProductoRepositorio;
        private readonly IEstadoCarritoRepositorio _edoCarritoRepositorio;
        private readonly IRepositorio<Pago> _pagoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        protected ApiResponse _response;

        public CarritoCompraController(ILogger<CarritoCompraController> logger
            , ICarritoCompraRepositorio carritoRepositorio
            , ICarritoProductoRepositorio carritoProductoRepositorio
            , IEstadoCarritoRepositorio edoCarritoRepositorio
            , IRepositorio<Pago> pagoRepositorio
            , IUsuarioRepositorio usuarioRepositorio)
        {
            _logger = logger;
            _carritoRepositorio = carritoRepositorio;
            _carritoProductoRepositorio = carritoProductoRepositorio;
            _edoCarritoRepositorio = edoCarritoRepositorio;
            _pagoRepositorio = pagoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateCarrito([FromBody] CarritoCompraCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _carritoRepositorio.Obtener(c => c.CodigoCarrito.ToLower() == createDto.CodigoCarrito.ToLower()) != null)
                {
                    ModelState.AddModelError("codigo", "Este codigo para el carrito ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                if (createDto.CarritoProductoId <= 0
                    || createDto.EdoCarritoId <= 0
                    || createDto.PagoId <= 0
                    || createDto.UsuarioId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al crear el carrito",
                        "Es necesario hacer referencia al estado del carrito (activo o inactivo)" +
                        " o el pago que se esta realizando" +
                        " o al usuario dueño del carrito"
                    };
                    return BadRequest(_response);
                }

                var edoCarrito = await _edoCarritoRepositorio.Obtener(ed => ed.IdEdoCarrito == createDto.EdoCarritoId);
                var carritoProducto = await _carritoProductoRepositorio.Obtener(cp => cp.IdCarritoProducto == createDto.CarritoProductoId,
                incluir: query => query
                .Include(cp => cp.Subproducto)
                    .ThenInclude(sp => sp.Producto)
                        .ThenInclude(p => p.Categoria)
                .Include(cp => cp.Subproducto)
                    .ThenInclude(sp => sp.Producto)
                        .ThenInclude(p => p.Marca));
                var pago = await _pagoRepositorio.Obtener(p => p.IdPago == createDto.PagoId,
                    incluir: query => query
                    .Include(p => p.MetodoPago));
                var usuario = await _usuarioRepositorio.Obtener(u => u.IdUsuario == createDto.UsuarioId,
                    incluir: query => query
                    .Include(u => u.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(u => u.EstadoCuenta)
                    .Include(u => u.Genero));

                CarritoCompra modelo = new()
                {
                    CodigoCarrito = createDto.CodigoCarrito,
                    Descuento = createDto.Descuento,
                    Impuestos = carritoProducto.Subtotal * (20M / 100),
                    Total = carritoProducto.Subtotal + (carritoProducto.Subtotal * (20M / 100)) - createDto.Descuento,
                    CarritoProducto = carritoProducto,
                    EstadoCarrito = edoCarrito,
                    Pago = pago,
                    Usuario = usuario
                };

                await _carritoRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetCarrito", new { id = modelo.IdCarrito }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetCarritos()
        {
            try
            {
                IEnumerable<CarritoCompra> carritoList = await _carritoRepositorio.ObtenerTodos();

                IEnumerable<CarritoProducto> carritoProducto = await _carritoProductoRepositorio.ObtenerTodos(
                incluir: query => query
                    .Include(cp => cp.Subproducto)
                        .ThenInclude(sp => sp.Producto)
                            .ThenInclude(p => p.Categoria)
                    .Include(cp => cp.Subproducto)
                        .ThenInclude(sp => sp.Producto)
                            .ThenInclude(p => p.Marca));
                IEnumerable<EstadoCarrito> edoCarrito = await _edoCarritoRepositorio.ObtenerTodos();
                IEnumerable<Pago> pago = await _pagoRepositorio.ObtenerTodos(
                incluir: query => query.Include(p => p.MetodoPago));
                IEnumerable<Usuario> usuario = await _usuarioRepositorio.ObtenerTodos(
                incluir: query => query
                    .Include(u => u.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(c => c.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(u => u.EstadoCuenta)
                    .Include(u => u.Genero));

                _response.Resultado = carritoList.Select(c => new CarritoCompraGetDto
                {
                    IdCarrito = c.IdCarrito,
                    CodigoCarrito = c.CodigoCarrito,
                    Descuento = c.Descuento,
                    Impuestos = c.Impuestos,
                    Total = c.Total,
                    CarritoProducto = carritoProducto.FirstOrDefault(cp => cp.IdCarritoProducto == c.CarritoProductoId),
                    EstadoCarrito = edoCarrito.FirstOrDefault(ec => ec.IdEdoCarrito == c.EdoCarritoId),
                    Pago = pago.FirstOrDefault(p => p.IdPago == c.PagoId),
                    Usuario = usuario.FirstOrDefault(u => u.IdUsuario == c.UsuarioId)
                }).ToList();

                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string>
                {
                    StatusCode(StatusCodes.Status500InternalServerError).ToString()
                    , ex.ToString()
                };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetCarrito")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetCarrito(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener carrito con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var carrito = await _carritoRepositorio.Obtener(
                x => x.IdCarrito == id,
                incluir: query => query
                    .Include(c => c.CarritoProducto)
                        .ThenInclude(cp => cp.Subproducto)
                            .ThenInclude(sp => sp.Producto)
                                .ThenInclude(p => p.Categoria)
                    .Include(c => c.CarritoProducto)
                        .ThenInclude(cp => cp.Subproducto)
                            .ThenInclude(sp => sp.Producto)
                                .ThenInclude(p => p.Marca)
                    .Include(c => c.EstadoCarrito)
                    .Include(c => c.Pago).ThenInclude(p => p.MetodoPago)
                    .Include(c => c.Usuario).ThenInclude(u => u.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(c => c.Usuario).ThenInclude(u => u.EstadoCuenta)
                    .Include(c => c.Usuario).ThenInclude(u => u.Genero));

                if (carrito == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new CarritoCompraGetDto
                {
                    IdCarrito = carrito.IdCarrito,
                    CodigoCarrito = carrito.CodigoCarrito,
                    Descuento = carrito.Descuento,
                    Impuestos = carrito.Impuestos,
                    Total = carrito.Total,
                    CarritoProducto = carrito.CarritoProducto,
                    EstadoCarrito = carrito.EstadoCarrito,
                    Pago = carrito.Pago,
                    Usuario = carrito.Usuario
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

        [HttpGet("GetPorCodigoCarrito")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> BuscarCarritoPorCodigo([FromQuery] string codigo)
        {
            try
            {
                if (string.IsNullOrEmpty(codigo))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>() { "El parámetro 'codigo' es requerido." };
                    return BadRequest(_response);
                }

                IEnumerable<CarritoCompra> carritos = await _carritoRepositorio.ObtenerTodos(
                    c => c.CodigoCarrito.Contains(codigo),
                    incluir: query => query
                        .Include(c => c.CarritoProducto)
                            .ThenInclude(cp => cp.Subproducto)
                                .ThenInclude(sp => sp.Producto)
                                    .ThenInclude(p => p.Categoria)
                        .Include(c => c.CarritoProducto)
                            .ThenInclude(cp => cp.Subproducto)
                                .ThenInclude(sp => sp.Producto)
                                    .ThenInclude(p => p.Marca)
                        .Include(c => c.EstadoCarrito)
                        .Include(c => c.Pago)
                            .ThenInclude(p => p.MetodoPago)
                        .Include(c => c.Usuario)
                            .ThenInclude(u => u.Direccion)
                                .ThenInclude(d => d.Ciudad)
                                    .ThenInclude(ci => ci.Estado)
                                        .ThenInclude(e => e.Pais)
                        .Include(c => c.Usuario).ThenInclude(u => u.EstadoCuenta)
                        .Include(c => c.Usuario).ThenInclude(u => u.Genero));

                _response.Resultado = carritos;
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
        public async Task<IActionResult> UpdateCarrito(int id, [FromBody] CarritoCompraUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdCarrito)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                if (updateDto.CarritoProductoId <= 0
                    || updateDto.EdoCarritoId <= 0
                    || updateDto.PagoId <= 0
                    || updateDto.UsuarioId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar el carrito",
                        "Es necesario hacer referencia al estado del carrito (activo o inactivo)" +
                        " o el pago que se esta realizando" +
                        " o al usuario dueño del carrito"
                    };
                    return BadRequest(_response);
                }

                var carritoExistente = await _carritoRepositorio.Obtener(c => c.IdCarrito == id);
                if (carritoExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                var carritoProducto = await _carritoProductoRepositorio.Obtener(cp => cp.IdCarritoProducto == updateDto.CarritoProductoId);
                var edoCarrito = await _edoCarritoRepositorio.Obtener(ec => ec.IdEdoCarrito == updateDto.EdoCarritoId);
                var pago = await _pagoRepositorio.Obtener(p => p.IdPago == updateDto.PagoId);
                var usuario = await _usuarioRepositorio.Obtener(u => u.IdUsuario == updateDto.UsuarioId);

                carritoExistente.EdoCarritoId = updateDto.EdoCarritoId;
                carritoExistente.PagoId = updateDto.PagoId;
                carritoExistente.UsuarioId = updateDto.UsuarioId;

                await _carritoRepositorio.Actualizar(carritoExistente);

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
