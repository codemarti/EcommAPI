using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubproductoController : ControllerBase
    {
        private readonly ILogger<SubproductoController> _logger;
        private readonly ISubproductoRepositorio _subproductoRepositorio;
        private readonly IProductoRepositorio _productoRepositorio;
        private readonly ISucursalRepositorio _sucursalRepositorio;
        protected ApiResponse _response;

        public SubproductoController(ILogger<SubproductoController> logger
            , ISubproductoRepositorio subproductoRepositorio
            , IProductoRepositorio productoRepositorio
            , ISucursalRepositorio sucursalRepositorio)
        {
            _logger = logger;
            _subproductoRepositorio = subproductoRepositorio;
            _productoRepositorio = productoRepositorio;
            _sucursalRepositorio = sucursalRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateSubproducto([FromBody] SubproductoCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _subproductoRepositorio.Obtener(e => e.CodigoBarras.ToLower() == createDto.CodigoBarras.ToLower()) != null)
                {
                    ModelState.AddModelError("codigoExiste", "Error este codigo ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                if (createDto.ProductoId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion",
                        "Es necesario llenar a los campos que pertenecen al subproducto"
                    };
                    return BadRequest(_response);
                }

                var producto = await _productoRepositorio.Obtener(p => p.IdProducto == createDto.ProductoId,
                    incluir: query => query
                    .Include(p => p.Categoria)
                    .Include(p => p.Marca));

                var sucursal = await _sucursalRepositorio.Obtener(s => s.IdSucursal == createDto.SucursalId,
                    incluir: query => query
                    .Include(s => s.Cadena)
                        .ThenInclude(s => s.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                    .Include(s => s.Cadena)
                        .ThenInclude(ca => ca.TipoCadena)
                            .ThenInclude(tc => tc.ModeloNegocio)
                    .Include(s => s.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(es => es.Pais)
                    .Include(s => s.EstadoSucursal)
                    .Include(s => s.HorarioSucursal)
                    .Include(s => s.ServiciosSucursal));

                Subproducto modelo = new()
                {
                    CodigoBarras = createDto.CodigoBarras,
                    ImagenSub = createDto.ImagenSub,
                    Descripcion = createDto.Descripcion,
                    PrecioSub = createDto.PrecioSub,
                    FechaCreacion = DateTime.UtcNow,
                    Stock = createDto.Stock,
                    Producto = producto,
                    Sucursal = sucursal
                };

                await _subproductoRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetSubproducto", new { id = modelo.IdSubproducto }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetSubproductos()
        {
            IEnumerable<Subproducto> subproductoList = await _subproductoRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(s => s.Producto)
                    .ThenInclude(p => p.Categoria)
                .Include(s => s.Producto)
                    .ThenInclude(p => p.Marca)
                .Include(s => s.Sucursal)
                    .ThenInclude(s => s.Cadena)
                        .ThenInclude(s => s.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(es => es.Pais)
                .Include(s => s.Sucursal)
                    .ThenInclude(s => s.Cadena)
                        .ThenInclude(ca => ca.TipoCadena)
                            .ThenInclude(tc => tc.ModeloNegocio)
                .Include(s => s.Sucursal)
                    .ThenInclude(s => s.ServiciosSucursal)
                .Include(s => s.Sucursal)
                    .ThenInclude(s => s.EstadoSucursal)
                .Include(s => s.Sucursal)
                    .ThenInclude(s => s.HorarioSucursal));

            _response.Resultado = subproductoList.Select(sp => new SubproductoGetDto
            {
                IdSubproducto = sp.IdSubproducto,
                CodigoBarras = sp.CodigoBarras,
                ImagenSub = sp.ImagenSub,
                Descripcion = sp.Descripcion,
                PrecioSub = sp.PrecioSub,
                Stock = sp.Stock,
                FechaActualizacion = sp.FechaActualizacion,
                FechaCreacion = sp.FechaCreacion,
                Producto = sp.Producto,
                Sucursal = sp.Sucursal
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetSubproducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetSubproducto(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al subproducto con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var subproducto = await _subproductoRepositorio.Obtener(
                    x => x.IdSubproducto == id,
                    incluir: query => query
                        .Include(sp => sp.Producto)
                            .ThenInclude(p => p.Categoria)
                        .Include(sp => sp.Producto)
                            .ThenInclude(p => p.Marca)
                        .Include(s => s.Sucursal)
                            .ThenInclude(s => s.Cadena)
                                .ThenInclude(s => s.Direccion)
                                    .ThenInclude(d => d.Ciudad)
                                        .ThenInclude(ci => ci.Estado)
                                            .ThenInclude(es => es.Pais)
                        .Include(s => s.Sucursal)
                            .ThenInclude(s => s.Cadena)
                                .ThenInclude(ca => ca.TipoCadena)
                                    .ThenInclude(tc => tc.ModeloNegocio)
                        .Include(s => s.Sucursal)
                            .ThenInclude(s => s.ServiciosSucursal)
                        .Include(s => s.Sucursal)
                            .ThenInclude(s => s.EstadoSucursal)
                        .Include(s => s.Sucursal)
                            .ThenInclude(s => s.HorarioSucursal));

                if (subproducto == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new SubproductoGetDto
                {
                    IdSubproducto = id,
                    CodigoBarras = subproducto.CodigoBarras,
                    ImagenSub = subproducto.ImagenSub,
                    Descripcion = subproducto.Descripcion,
                    PrecioSub = subproducto.PrecioSub,
                    Stock = subproducto.Stock,
                    FechaActualizacion = subproducto.FechaActualizacion,
                    FechaCreacion = subproducto.FechaCreacion,
                    Producto = subproducto.Producto,
                    Sucursal = subproducto.Sucursal
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

        [HttpGet("GetSubproductosPorRegistros", Name = "GetPaginadoSubproductos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetPaginadoSubproductos(string dataSubproducto,
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
                IEnumerable<Subproducto> subproductoList = await _subproductoRepositorio
                    .ObtenerTodos(sp => sp.CodigoBarras.Contains(dataSubproducto)
                    || sp.Producto.NombreProducto.Contains(dataSubproducto)
                    || sp.Sucursal.NombreSucursal.Contains(dataSubproducto)
                    || sp.Sucursal.RFCS.Contains(dataSubproducto),
                    incluir: query => query
                    .Include(sp => sp.Producto)
                        .ThenInclude(p => p.Categoria)
                    .Include(sp => sp.Producto)
                        .ThenInclude(p => p.Marca)
                    .Include(s => s.Sucursal)
                        .ThenInclude(s => s.Cadena)
                            .ThenInclude(s => s.Direccion)
                                .ThenInclude(d => d.Ciudad)
                                    .ThenInclude(ci => ci.Estado)
                                        .ThenInclude(es => es.Pais)
                    .Include(s => s.Sucursal)
                        .ThenInclude(s => s.Cadena)
                            .ThenInclude(ca => ca.TipoCadena)
                                .ThenInclude(tc => tc.ModeloNegocio)
                    .Include(s => s.Sucursal)
                        .ThenInclude(s => s.ServiciosSucursal)
                    .Include(s => s.Sucursal)
                        .ThenInclude(s => s.EstadoSucursal)
                    .Include(s => s.Sucursal)
                        .ThenInclude(s => s.HorarioSucursal));

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
        #region " APARTADO UPDATE "
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSubproducto(int id, [FromBody] SubproductoUpdateDto updateDto)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.ProductoId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar el subproducto",
                        "Es necesario hacer referencia a los campos del subproducto"
                    };
                    return BadRequest(_response);
                }

                var subproductoExistente = await _subproductoRepositorio.Obtener(s => s.IdSubproducto == id);
                if (subproductoExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                subproductoExistente.CodigoBarras = updateDto.CodigoBarras;
                subproductoExistente.ImagenSub = updateDto.ImagenSub;
                subproductoExistente.Descripcion = updateDto.Descripcion;
                subproductoExistente.PrecioSub = updateDto.PrecioSub;
                subproductoExistente.Stock = updateDto.Stock;
                subproductoExistente.ProductoId = updateDto.ProductoId;
                subproductoExistente.SucursalId = updateDto.SucursalId;

                await _subproductoRepositorio.Actualizar(subproductoExistente);
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
        public async Task<IActionResult> DeleteSubproducto(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var subproducto = await _subproductoRepositorio.Obtener(x => x.IdSubproducto == id);
                if (subproducto == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _subproductoRepositorio.Remover(subproducto);
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
