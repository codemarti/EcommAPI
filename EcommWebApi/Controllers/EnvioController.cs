using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvioController : ControllerBase
    {
        private readonly IEnvioRepositorio _envioRepositorio;
        private readonly IDireccionRepositorio _direccionRepositorio;
        private readonly IMetodoEnvioRepositorio _mtdEnvioRepositorio;
        private readonly IVentaRepositorio _ventaRepositorio;
        protected ApiResponse _response;

        public EnvioController(IEnvioRepositorio envioRepositorio
            , IDireccionRepositorio direccionRepositorio
            , IMetodoEnvioRepositorio mtdEnvioRepositorio
            , IVentaRepositorio ventaRepositorio)
        {
            _envioRepositorio = envioRepositorio;
            _direccionRepositorio = direccionRepositorio;
            _mtdEnvioRepositorio = mtdEnvioRepositorio;
            _ventaRepositorio = ventaRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateEnvio([FromBody] EnvioCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _envioRepositorio.Obtener(p => p.ReferenciaEnvio.ToLower() == createDto.ReferenciaEnvio.ToLower()) != null)
                {
                    ModelState.AddModelError("referenciaExiste", "Esta referencia ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto.ReferenciaEnvio == null)
                    return BadRequest();

                if (createDto is null)
                    return BadRequest();

                if (createDto.DireccionId <= 0
                    || createDto.EdoEnvioId <= 0
                    || createDto.MetodoEnvioId <= 0
                    || createDto.VentaId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion",
                        "Es necesario llenar a los campos del envio"
                    };
                    return BadRequest(_response);
                }

                var direccion = await _direccionRepositorio.Obtener(d => d.IdDireccion == createDto.DireccionId,
                    incluir: query => query
                    .Include(dirEnv => dirEnv.Ciudad)
                        .ThenInclude(ci => ci.Estado)
                            .ThenInclude(es => es.Pais));
                var mtdEnvio = await _mtdEnvioRepositorio.Obtener(ee => ee.IdMetodoEnvio == createDto.MetodoEnvioId);
                var venta = await _ventaRepositorio.Obtener(v => v.IdVenta == createDto.VentaId,
                incluir: query => query
                    .Include(cc => cc.CarritoCompra)
                        .ThenInclude(cc => cc.CarritoProducto)
                            .ThenInclude(cp => cp.Subproducto)
                                .ThenInclude(s => s.Producto)
                                    .ThenInclude(p => p.Categoria)
                    .Include(cc => cc.CarritoCompra)
                        .ThenInclude(cc => cc.CarritoProducto)
                            .ThenInclude(cp => cp.Subproducto)
                                .ThenInclude(s => s.Producto)
                                    .ThenInclude(p => p.Marca)
                    .Include(v => v.CarritoCompra)
                        .ThenInclude(cc => cc.EstadoCarrito)
                    .Include(cp => cp.CarritoCompra)
                        .ThenInclude(cc => cc.Pago)
                            .ThenInclude(p => p.MetodoPago)
                    .Include(cc => cc.CarritoCompra)
                        .ThenInclude(cc => cc.Usuario)
                            .ThenInclude(u => u.Direccion)
                                .ThenInclude(dirUser => dirUser.Ciudad)
                                    .ThenInclude(c => c.Estado)
                                        .ThenInclude(es => es.Pais)
                    .Include(cc => cc.CarritoCompra)
                        .ThenInclude(cc => cc.Usuario)
                            .ThenInclude(u => u.EstadoCuenta)
                    .Include(cc => cc.CarritoCompra)
                        .ThenInclude(cc => cc.Usuario)
                            .ThenInclude(u => u.Genero)
                        .Include(v => v.Empleado)
                            .ThenInclude(emp => emp.Direccion)
                                .ThenInclude(dirEmp => dirEmp.Ciudad)
                                    .ThenInclude(ci => ci.Estado)
                                        .ThenInclude(es => es.Pais)
                        .Include(v => v.Empleado)
                            .ThenInclude(emp => emp.Rol)
                        .Include(v => v.EstadoPago));

                Envio modelo = new()
                {
                    ReferenciaEnvio = createDto.ReferenciaEnvio,
                    Direccion = direccion,
                    MetodoEnvio = mtdEnvio,
                    Venta = venta
                };

                await _envioRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetPorReferenciaEnvio", new { referencia = modelo.ReferenciaEnvio }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetEnvios()
        {
            IEnumerable<Envio> envioList = await _envioRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(e => e.Direccion)
                    .ThenInclude(dirEnv => dirEnv.Ciudad)
                        .ThenInclude(ci => ci.Estado)
                            .ThenInclude(es => es.Pais)
                .Include(e => e.MetodoEnvio)
                .Include(e => e.Venta)
                    .ThenInclude(v => v.CarritoCompra)
                        .ThenInclude(cc => cc.CarritoProducto)
                            .ThenInclude(cc => cc.Subproducto)
                                .ThenInclude(s => s.Producto)
                                    .ThenInclude(p => p.Categoria)
                .Include(e => e.Venta)
                    .ThenInclude(v => v.CarritoCompra)
                        .ThenInclude(cc => cc.CarritoProducto)
                            .ThenInclude(cc => cc.Subproducto)
                                .ThenInclude(s => s.Producto)
                                    .ThenInclude(p => p.Marca)
                .Include(e => e.Venta)
                    .ThenInclude(v => v.CarritoCompra)
                        .ThenInclude(cc=>cc.EstadoCarrito)
                .Include(e => e.Venta)
                    .ThenInclude(cp => cp.CarritoCompra)
                        .ThenInclude(cc => cc.Pago)
                            .ThenInclude(p => p.MetodoPago)
                .Include(e => e.Venta)
                    .ThenInclude(cp => cp.CarritoCompra)
                        .ThenInclude(cc => cc.Usuario)
                            .ThenInclude(u => u.Direccion)
                                .ThenInclude(dirUser => dirUser.Ciudad)
                                    .ThenInclude(c => c.Estado)
                                        .ThenInclude(es => es.Pais)
                .Include(e => e.Venta)
                    .ThenInclude(cp => cp.CarritoCompra)
                        .ThenInclude(cc => cc.Usuario)
                            .ThenInclude(u => u.EstadoCuenta)
                .Include(e => e.Venta)
                    .ThenInclude(cp => cp.CarritoCompra)
                        .ThenInclude(cc => cc.Usuario)
                            .ThenInclude(u => u.Genero)
                .Include(e => e.Venta)
                    .ThenInclude(v => v.Empleado)
                        .ThenInclude(emp => emp.Direccion)
                            .ThenInclude(dirEmp => dirEmp.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                .Include(e => e.Venta)
                    .ThenInclude(v => v.Empleado)
                        .ThenInclude(emp => emp.Rol)
                .Include(e => e.Venta)
                    .ThenInclude(v => v.EstadoPago));

            _response.Resultado = envioList.Select(e => new EnvioGetDto
            {
                IdEnvio = e.IdEnvio,
                ReferenciaEnvio = e.ReferenciaEnvio,
                Direccion = e.Direccion,
                MetodoEnvio = e.MetodoEnvio,
                Venta = e.Venta
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("BuscarEnviosPorReferencia", Name = "GetPorReferenciaEnvio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetPorReferenciaEnvio([FromQuery] string referencia)
        {
            try
            {
                if (string.IsNullOrEmpty(referencia))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>() { "El parámetro 'referencia' es requerido." };
                    return BadRequest(_response);
                }

                IEnumerable<Envio> envios = await _envioRepositorio.ObtenerTodos(
                    e => e.ReferenciaEnvio.Contains(referencia),
                    incluir: query => query
                .Include(e => e.Direccion)
                    .ThenInclude(dirEnv => dirEnv.Ciudad)
                        .ThenInclude(ci => ci.Estado)
                            .ThenInclude(es => es.Pais)
                .Include(e => e.MetodoEnvio)
                .Include(e => e.Venta)
                    .ThenInclude(v => v.CarritoCompra)
                        .ThenInclude(cc => cc.CarritoProducto)
                            .ThenInclude(cc => cc.Subproducto)
                                .ThenInclude(s => s.Producto)
                                    .ThenInclude(p => p.Categoria)
                .Include(e => e.Venta)
                    .ThenInclude(v => v.CarritoCompra)
                        .ThenInclude(cc => cc.CarritoProducto)
                            .ThenInclude(cc => cc.Subproducto)
                                .ThenInclude(s => s.Producto)
                                    .ThenInclude(p => p.Marca)
                .Include(e => e.Venta)
                    .ThenInclude(v => v.CarritoCompra)
                        .ThenInclude(cc => cc.EstadoCarrito)
                .Include(e => e.Venta)
                    .ThenInclude(cp => cp.CarritoCompra)
                        .ThenInclude(cc => cc.Pago)
                            .ThenInclude(p => p.MetodoPago)
                .Include(e => e.Venta)
                    .ThenInclude(cp => cp.CarritoCompra)
                        .ThenInclude(cc => cc.Usuario)
                            .ThenInclude(u => u.Direccion)
                                .ThenInclude(dirUser => dirUser.Ciudad)
                                    .ThenInclude(c => c.Estado)
                                        .ThenInclude(es => es.Pais)
                .Include(e => e.Venta)
                    .ThenInclude(cp => cp.CarritoCompra)
                        .ThenInclude(cc => cc.Usuario)
                            .ThenInclude(u => u.EstadoCuenta)
                .Include(e => e.Venta)
                    .ThenInclude(cp => cp.CarritoCompra)
                        .ThenInclude(cc => cc.Usuario)
                            .ThenInclude(u => u.Genero)
                .Include(e => e.Venta)
                    .ThenInclude(v => v.Empleado)
                        .ThenInclude(emp => emp.Direccion)
                            .ThenInclude(dirEmp => dirEmp.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                .Include(e => e.Venta)
                    .ThenInclude(v => v.Empleado)
                        .ThenInclude(emp => emp.Rol)
                .Include(e => e.Venta)
                    .ThenInclude(v => v.EstadoPago));

                _response.Resultado = envios;
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
        [HttpPut("UpdateReferenciaEnvio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEnvio(string referencia, [FromBody] EnvioUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || referencia != updateDto.ReferenciaEnvio)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.DireccionId <= 0
                    || updateDto.EdoEnvioId <= 0
                    || updateDto.MetodoEnvioId <= 0
                    || updateDto.VentaId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar el envio",
                        "Es necesario hacer referencia a los campos de envio"
                    };
                    return BadRequest(_response);
                }

                var envioExistente = await _envioRepositorio.Obtener(cp => cp.ReferenciaEnvio == referencia);
                if (envioExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                envioExistente.ReferenciaEnvio = referencia;
                envioExistente.DireccionId = updateDto.DireccionId;
                envioExistente.MetodoEnvioId = updateDto.MetodoEnvioId;
                envioExistente.VentaId = updateDto.VentaId;

                await _envioRepositorio.Actualizar(envioExistente);
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
        [HttpDelete("DeleteReferenciaEnvio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEnvio(string referencia)
        {
            try
            {
                if (referencia == null || referencia == "")
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var envio = await _envioRepositorio.Obtener(x => x.ReferenciaEnvio == referencia);
                if (envio == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _envioRepositorio.Remover(envio);
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
