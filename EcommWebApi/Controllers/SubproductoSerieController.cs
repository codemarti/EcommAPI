namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubproductoSerieController : ControllerBase
    {
        private readonly ILogger<SubproductoSerieController> _logger;
        private readonly ISubproductoSerieRepositorio _subproductoSerieRepositorio;
        private readonly ISubproductoRepositorio _subproductoRepositorio;
        protected ApiResponse _response;

        public SubproductoSerieController(ILogger<SubproductoSerieController> logger
            , ISubproductoSerieRepositorio subproductoSerieRepositorio
            , ISubproductoRepositorio subproductoRepositorio)
        {
            _logger = logger;
            _subproductoSerieRepositorio = subproductoSerieRepositorio;
            _subproductoRepositorio = subproductoRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateSubproductoSerie([FromBody] SubproductoSerieCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _subproductoSerieRepositorio.Obtener(e => e.NumeroSerie.ToLower() == createDto.NumeroSerie.ToLower()) != null)
                {
                    ModelState.AddModelError("codigoExiste", "Error este codigo ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                if (createDto.SubproductoId <= 0)
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

                var subproducto = await _subproductoRepositorio.Obtener(p => p.IdSubproducto == createDto.SubproductoId,
                    incluir: query => query
                    .Include(sp => sp.Producto)
                        .ThenInclude(p=>p.Categoria)
                    .Include(sp => sp.Producto)
                        .ThenInclude(p => p.Marca)
                    );

                SubproductoSerie modelo = new()
                {
                    NumeroSerie = createDto.NumeroSerie,
                    Subproducto = subproducto
                };

                await _subproductoSerieRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetSubproductoSerie", new { id = modelo.IdSerie }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetSubproductosSerie()
        {
            IEnumerable<SubproductoSerie> subproductoSerieList = await _subproductoSerieRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Producto)
                        .ThenInclude(p => p.Categoria)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Producto)
                        .ThenInclude(p => p.Marca)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.Cadena)
                            .ThenInclude(s => s.Direccion)
                                .ThenInclude(d => d.Ciudad)
                                    .ThenInclude(ci => ci.Estado)
                                        .ThenInclude(es => es.Pais)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.Cadena)
                            .ThenInclude(ca => ca.TipoCadena)
                                .ThenInclude(tc => tc.ModeloNegocio)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.ServiciosSucursal)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.EstadoSucursal)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.HorarioSucursal));

            _response.Resultado = subproductoSerieList.Select(ss => new SubproductoSerieGetDto
            {
                IdSerie = ss.IdSerie,
                NumeroSerie = ss.NumeroSerie,
                Subproducto = ss.Subproducto
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetSubproductoSerie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetSubproductoSerie(int id)
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

                var subproductoSerie = await _subproductoSerieRepositorio.Obtener(
                x => x.IdSerie == id,
                incluir: query => query
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Producto)
                        .ThenInclude(p => p.Categoria)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Producto)
                        .ThenInclude(p => p.Marca)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.Cadena)
                            .ThenInclude(s => s.Direccion)
                                .ThenInclude(d => d.Ciudad)
                                    .ThenInclude(ci => ci.Estado)
                                        .ThenInclude(es => es.Pais)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.Cadena)
                            .ThenInclude(ca => ca.TipoCadena)
                                .ThenInclude(tc => tc.ModeloNegocio)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.ServiciosSucursal)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.EstadoSucursal)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.HorarioSucursal));

                if (subproductoSerie == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new SubproductoSerieGetDto
                {
                    IdSerie = id,
                    NumeroSerie = subproductoSerie.NumeroSerie,
                    Subproducto = subproductoSerie.Subproducto
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

        [HttpGet("GetSubproductosSeriePorRegistros", Name = "GetPaginadoSubproductosSerie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetPaginadoSubproductosSerie(string numSerie,
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
                IEnumerable<SubproductoSerie> subproductoSerieList = await _subproductoSerieRepositorio
                .ObtenerTodos(sp => sp.NumeroSerie.Contains(numSerie),
                incluir: query => query
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Producto)
                        .ThenInclude(p => p.Categoria)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Producto)
                        .ThenInclude(p => p.Marca)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.Cadena)
                            .ThenInclude(s => s.Direccion)
                                .ThenInclude(d => d.Ciudad)
                                    .ThenInclude(ci => ci.Estado)
                                        .ThenInclude(es => es.Pais)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.Cadena)
                            .ThenInclude(ca => ca.TipoCadena)
                                .ThenInclude(tc => tc.ModeloNegocio)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.ServiciosSucursal)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.EstadoSucursal)
                .Include(ss => ss.Subproducto)
                    .ThenInclude(sp => sp.Sucursal)
                        .ThenInclude(s => s.HorarioSucursal));

                // Creo mi total de registros dependiente de la línea anterior
                decimal totalRegistros = subproductoSerieList.Count();
                int totalPaginas = Convert.ToInt32(Math.Ceiling(totalRegistros / registros));
                var productos = subproductoSerieList.Skip((_pagina - 1) * registros).Take(registros).ToList();

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
        public async Task<IActionResult> UpdateSubproductoSerie(int id, [FromBody] SubproductoSerieUpdateDto updateDto)
        {
            try
            {
                if (id == 0)
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
                        "Error de peticion al actualizar el subproducto",
                        "Es necesario hacer referencia a los campos del subproducto"
                    };
                    return BadRequest(_response);
                }

                var subproductoSerieExistente = await _subproductoSerieRepositorio.Obtener(s => s.IdSerie == id);
                if (subproductoSerieExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                subproductoSerieExistente.NumeroSerie = updateDto.NumeroSerie;
                subproductoSerieExistente.SubproductoId = updateDto.SubproductoId;

                await _subproductoSerieRepositorio.Actualizar(subproductoSerieExistente);
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
        public async Task<IActionResult> DeleteSubproductoSerie(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var subproductoSerie = await _subproductoSerieRepositorio.Obtener(x => x.IdSerie == id);
                if (subproductoSerie == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _subproductoSerieRepositorio.Remover(subproductoSerie);
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
