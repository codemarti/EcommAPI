using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly ILogger<VentaController> _logger;
        private readonly IVentaRepositorio _ventaRepositorio;
        private readonly ICarritoCompraRepositorio _carritoRepositorio;
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        private readonly IEstadoPagoRepositorio _edoPagoRepositorio;
        protected ApiResponse _response;

        public VentaController(ILogger<VentaController> logger
            , IVentaRepositorio ventaRepositorio
            , ICarritoCompraRepositorio carritoRepositorio
            , IEmpleadoRepositorio empleadoRepositorio
            , IEstadoPagoRepositorio edoPagoRepositorio)
        {
            _logger = logger;
            _ventaRepositorio = ventaRepositorio;
            _carritoRepositorio = carritoRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
            _edoPagoRepositorio = edoPagoRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateVenta([FromBody] VentaCreateDto createDto)
        {
            try
            {
                if (createDto is null)
                    return BadRequest();

                if (createDto.CarritoId <= 0
                    || createDto.EmpleadoId <= 0
                    || createDto.EdoPagoId <= 0)
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

                var carritoCompra = await _carritoRepositorio.Obtener(cp => cp.IdCarrito == createDto.CarritoId,
                    incluir: query => query
                    .Include(cc => cc.CarritoProducto)
                        .ThenInclude(cp => cp.Subproducto)
                            .ThenInclude(sp => sp.Producto)
                                .ThenInclude(p => p.Categoria)
                    .Include(cc => cc.CarritoProducto)
                        .ThenInclude(cp => cp.Subproducto)
                            .ThenInclude(sp => sp.Producto)
                                .ThenInclude(p => p.Marca)
                    .Include(cc => cc.EstadoCarrito)
                    .Include(cc => cc.Pago)
                        .ThenInclude(p => p.MetodoPago)
                    .Include(cc => cc.Usuario)
                        .ThenInclude(u => u.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                    .Include(cc => cc.Usuario)
                        .ThenInclude(u => u.EstadoCuenta)
                    .Include(cc => cc.Usuario)
                        .ThenInclude(u => u.Genero));
                var empleado = await _empleadoRepositorio.Obtener(sc => sc.IdEmpleado == createDto.EmpleadoId,
                    incluir: query => query
                    .Include(em => em.Direccion)
                        .ThenInclude(de => de.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(em => em.Rol));
                var edoPago = await _edoPagoRepositorio.Obtener(ep => ep.IdEdoPago == createDto.EdoPagoId);

                Venta modelo = new()
                {
                    FechaRealizada = DateTime.UtcNow,
                    FechaDevolucion = createDto.FechaDevolucion,
                    Comentarios = createDto.Comentarios,
                    CarritoCompra = carritoCompra,
                    Empleado = empleado,
                    EstadoPago = edoPago
                };

                await _ventaRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVenta", new { id = modelo.IdVenta }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetVentas()
        {
            IEnumerable<Venta> ventaList = await _ventaRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.CarritoProducto)
                        .ThenInclude(cp => cp.Subproducto)
                            .ThenInclude(sp => sp.Producto)
                                .ThenInclude(p => p.Categoria)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.CarritoProducto)
                        .ThenInclude(cp => cp.Subproducto)
                            .ThenInclude(sp => sp.Producto)
                                .ThenInclude(p => p.Marca)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.EstadoCarrito)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.Pago)
                        .ThenInclude(p => p.MetodoPago)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.Usuario)
                        .ThenInclude(u => u.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.Usuario)
                        .ThenInclude(u => u.EstadoCuenta)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.Usuario)
                        .ThenInclude(u => u.Genero)
                .Include(v => v.Empleado)
                    .ThenInclude(em => em.Direccion)
                        .ThenInclude(de => de.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                .Include(v => v.Empleado)
                    .ThenInclude(em => em.Rol)
                .Include(ep => ep.EstadoPago));

            _response.Resultado = ventaList.Select(v => new VentaGetDto
            {
                IdVenta = v.IdVenta,
                FechaRealizada = v.FechaRealizada,
                FechaDevolucion = v.FechaDevolucion,
                Comentarios = v.Comentarios,
                CarritoCompra = v.CarritoCompra,
                Empleado = v.Empleado,
                EstadoPago = v.EstadoPago
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetVenta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetVenta(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al venta con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var venta = await _ventaRepositorio.Obtener(
                x => x.IdVenta == id,
                incluir: query => query
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.CarritoProducto)
                        .ThenInclude(cp => cp.Subproducto)
                            .ThenInclude(sp => sp.Producto)
                                .ThenInclude(p => p.Categoria)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.CarritoProducto)
                        .ThenInclude(cp => cp.Subproducto)
                            .ThenInclude(sp => sp.Producto)
                                .ThenInclude(p => p.Marca)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.EstadoCarrito)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.Pago)
                        .ThenInclude(p => p.MetodoPago)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.Usuario)
                        .ThenInclude(u => u.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.Usuario)
                        .ThenInclude(u => u.EstadoCuenta)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.Usuario)
                        .ThenInclude(u => u.Genero)
                .Include(v => v.Empleado)
                    .ThenInclude(em => em.Direccion)
                        .ThenInclude(de => de.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                .Include(v => v.Empleado)
                    .ThenInclude(em => em.Rol)
                .Include(ep => ep.EstadoPago));

                if (venta == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new VentaGetDto
                {
                    IdVenta = venta.IdVenta,
                    FechaRealizada = venta.FechaRealizada,
                    FechaDevolucion = venta.FechaDevolucion,
                    Comentarios = venta.Comentarios,
                    CarritoCompra = venta.CarritoCompra,
                    Empleado = venta.Empleado,
                    EstadoPago = venta.EstadoPago
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

        [HttpGet("GetVentasPorDataEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetVentasPorDataEmpleado([FromQuery] string datosEmpleado)
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

                IEnumerable<Venta> ventas = await _ventaRepositorio.ObtenerTodos(
                e => e.Empleado.RFCE.Contains(datosEmpleado)
                || e.Empleado.NumEmpleado.Contains(datosEmpleado)
                || (e.Empleado.NombreEmpleado + " " + e.Empleado.ApellidoEmpleado).Contains(datosEmpleado),
                    incluir: query => query
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.CarritoProducto)
                        .ThenInclude(cp => cp.Subproducto)
                            .ThenInclude(sp => sp.Producto)
                                .ThenInclude(p => p.Categoria)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.CarritoProducto)
                        .ThenInclude(cp => cp.Subproducto)
                            .ThenInclude(sp => sp.Producto)
                                .ThenInclude(p => p.Marca)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.EstadoCarrito)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.Pago)
                        .ThenInclude(p => p.MetodoPago)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.Usuario)
                        .ThenInclude(u => u.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.Usuario)
                        .ThenInclude(u => u.EstadoCuenta)
                .Include(v => v.CarritoCompra)
                    .ThenInclude(cc => cc.Usuario)
                        .ThenInclude(u => u.Genero)
                .Include(v => v.Empleado)
                    .ThenInclude(em => em.Direccion)
                        .ThenInclude(de => de.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                .Include(v => v.Empleado)
                    .ThenInclude(em => em.Rol)
                .Include(ep => ep.EstadoPago));

                if (ventas == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }


                if (ventas == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = ventas;
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
        public async Task<IActionResult> UpdateVenta(int id, [FromBody] VentaUpdateDto updateDto)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                if (updateDto.CarritoId <= 0
                    || updateDto.EmpleadoId <= 0
                    || updateDto.EdoPagoId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar la venta",
                        "Es necesario hacer referencia a los campos de la venta"
                    };
                    return BadRequest(_response);
                }

                var ventaExistente = await _ventaRepositorio.Obtener(v => v.IdVenta == id);
                if (ventaExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                ventaExistente.FechaDevolucion = updateDto.FechaDevolucion;
                ventaExistente.Comentarios = updateDto.Comentarios;
                ventaExistente.CarritoId = updateDto.CarritoId;
                ventaExistente.EmpleadoId = updateDto.EmpleadoId;
                ventaExistente.EdoPagoId = updateDto.EdoPagoId;

                await _ventaRepositorio.Actualizar(ventaExistente);
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
        public async Task<IActionResult> DeleteVenta(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var venta = await _ventaRepositorio.Obtener(x => x.IdVenta == id);
                if (venta == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _ventaRepositorio.Remover(venta);
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
