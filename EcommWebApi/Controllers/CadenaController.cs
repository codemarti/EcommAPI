using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadenaController : ControllerBase
    {
        private readonly ILogger<CadenaController> _logger;
        private readonly ICadenaRepositorio _cadenaRepositorio;
        private readonly IDireccionRepositorio _direccionRepositorio;
        private readonly ITipoCadenaRepositorio _tipoCadenaRepositorio;
        protected ApiResponse _response;

        public CadenaController(ILogger<CadenaController> logger
            , ICadenaRepositorio cadenaRepositorio
            , IDireccionRepositorio direccionRepositorio
            , ITipoCadenaRepositorio tipoCadenaRepositorio)
        {
            _logger = logger;
            _cadenaRepositorio = cadenaRepositorio;
            _direccionRepositorio = direccionRepositorio;
            _tipoCadenaRepositorio = tipoCadenaRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE"
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateCadena([FromBody] CadenaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _cadenaRepositorio.Obtener(c => c.RFCC.ToLower() == createDto.RFCC.ToLower()) != null)
                {
                    ModelState.AddModelError("rfcExiste", "Este rfc de cadena ya existe");
                    return BadRequest(ModelState);
                }

                if (await _cadenaRepositorio.Obtener(c => c.NombreCadena.ToLower() == createDto.NombreCadena.ToLower()) != null)
                {
                    ModelState.AddModelError("nombreExiste", "El nombre de la cadena ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                if (createDto.DireccionId <= 0 || createDto.TipoCadenaId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al crear la cadena",
                        "Es necesario hacer referencia a un tipo de cadena o direccion de la cadena"
                    };
                    return BadRequest(_response);
                }

                var direccion = await _direccionRepositorio.Obtener(d => d.IdDireccion == createDto.DireccionId,
                    incluir: query => query
                    .Include(d => d.Ciudad)
                        .ThenInclude(c => c.Estado)
                            .ThenInclude(e => e.Pais));
                var tipoCadena = await _tipoCadenaRepositorio.Obtener(tc => tc.IdTipoCadena == createDto.TipoCadenaId,
                    incluir: query => query
                    .Include(tc => tc.ModeloNegocio));

                Cadena modelo = new()
                {
                    RFCC = createDto.RFCC,
                    NombreCadena = createDto.NombreCadena,
                    Imagen = createDto.Imagen,
                    PPC = createDto.PPC,
                    TCC = createDto.TCC,
                    Direccion = direccion,
                    TipoCadena = tipoCadena
                };

                await _cadenaRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetCadena", new { id = modelo.IdCadena }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetCadenas()
        {
            try
            {
                IEnumerable<Cadena> cadenaList = await _cadenaRepositorio.ObtenerTodos();

                IEnumerable<Direccion> direcciones = await _direccionRepositorio.ObtenerTodos(
                    incluir: query => query
                        .Include(d => d.Ciudad)
                            .ThenInclude(c => c.Estado)
                                .ThenInclude(e => e.Pais));

                IEnumerable<TipoCadena> tipoCadena = await _tipoCadenaRepositorio.ObtenerTodos(
                    incluir: query => query
                        .Include(tc => tc.ModeloNegocio));

                _response.Resultado = cadenaList.Select(c => new CadenaGetDto
                {
                    IdCadena = c.IdCadena,
                    RFCC = c.RFCC,
                    NombreCadena = c.NombreCadena,
                    Imagen = c.Imagen,
                    PPC = c.PPC,
                    TCC = c.TCC,
                    Direccion = direcciones.FirstOrDefault(d => d.IdDireccion == c.DireccionId),
                    TipoCadena = tipoCadena.FirstOrDefault(tp => tp.IdTipoCadena == c.TipoCadenaId)
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

        [HttpGet("{id:int}", Name = "GetCadena")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetCadena(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener cadena con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var cadena = await _cadenaRepositorio.Obtener(
                    x => x.IdCadena == id,
                    incluir: query => query
                        .Include(c => c.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(e => e.Pais)
                        .Include(c => c.TipoCadena).ThenInclude(tc => tc.ModeloNegocio)
                );

                if (cadena == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new CadenaGetDto
                {
                    IdCadena = cadena.IdCadena,
                    RFCC = cadena.RFCC,
                    NombreCadena = cadena.NombreCadena,
                    Imagen = cadena.Imagen,
                    PPC = cadena.PPC,
                    TCC = cadena.TCC,
                    Direccion = cadena.Direccion,
                    TipoCadena = cadena.TipoCadena
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

        [HttpGet("GetPorNombreCadena")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> BuscarCadenasPorNombre([FromQuery] string nombre)
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

                IEnumerable<Cadena> cadenas = await _cadenaRepositorio.ObtenerTodos(
                    c => c.NombreCadena.Contains(nombre),
                    incluir: query => query
                        .Include(c => c.Direccion)
                            .ThenInclude(d => d.Ciudad)
                                .ThenInclude(ci => ci.Estado)
                                    .ThenInclude(e => e.Pais)
                        .Include(c => c.TipoCadena)
                            .ThenInclude(tc => tc.ModeloNegocio));

                _response.Resultado = cadenas;
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
        public async Task<IActionResult> UpdateCadena(int id, [FromBody] CadenaUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdCadena)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.DireccionId <= 0 || updateDto.TipoCadenaId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar la cadena",
                        "Es necesario hacer referencia a un tipo de cadena o direccion de la cadena"
                    };
                    return BadRequest(_response);
                }

                var cadenaExistente = await _cadenaRepositorio.Obtener(c => c.IdCadena == id);
                if (cadenaExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                var direccion = await _direccionRepositorio.Obtener(d => d.IdDireccion == updateDto.DireccionId);
                var tipoCadena = await _tipoCadenaRepositorio.Obtener(tp => tp.IdTipoCadena == updateDto.TipoCadenaId);

                cadenaExistente.RFCC = updateDto.RFCC;
                cadenaExistente.NombreCadena = updateDto.NombreCadena;
                cadenaExistente.Imagen = updateDto.Imagen;
                cadenaExistente.PPC = updateDto.PPC;
                cadenaExistente.TCC = updateDto.TCC;
                cadenaExistente.DireccionId = updateDto.DireccionId;
                cadenaExistente.TipoCadenaId = updateDto.TipoCadenaId;

                await _cadenaRepositorio.Actualizar(cadenaExistente);

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
