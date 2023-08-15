using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoController : ControllerBase
    {
        private readonly ILogger<EstadoController> _logger;
        private readonly IEstadoRepositorio _estadoRepositorio;
        private readonly IPaisRepositorio _paisRepositorio;
        protected ApiResponse _response;
        public EstadoController(ILogger<EstadoController> logger
            , IEstadoRepositorio estadoRepositorio
            , IPaisRepositorio paisRepositorio)
        {
            _logger = logger;
            _estadoRepositorio = estadoRepositorio;
            _paisRepositorio = paisRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateEstado([FromBody] EstadoCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _estadoRepositorio.Obtener(e => e.NombreEstado.ToLower() == createDto.NombreEstado.ToLower()) != null)
                {
                    ModelState.AddModelError("nombreExiste", "Error este nombre ya existe");
                    return BadRequest(ModelState);
                }

                if (await _estadoRepositorio.Obtener(e => e.CodigoEstado.ToLower() == createDto.CodigoEstado.ToLower()) != null)
                {
                    ModelState.AddModelError("codigoExiste", "Error este codigo ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                if (createDto.PaisId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion",
                        "Es necesario llenar a los campos que pertenecen a su estado o departamento"
                    };
                    return BadRequest(_response);
                }

                var pais = await _paisRepositorio.Obtener(p => p.IdPais == createDto.PaisId);

                Estado modelo = new()
                {
                    NombreEstado = createDto.NombreEstado,
                    CodigoEstado = createDto.CodigoEstado,
                    Pais = pais
                };

                await _estadoRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetEstado", new { id = modelo.IdEstado }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetEstados()
        {
            IEnumerable<Estado> estadoList = await _estadoRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(e => e.Pais));

            _response.Resultado = estadoList.Select(e => new EstadoGetDto
            {
                IdEstado = e.IdEstado,
                NombreEstado = e.NombreEstado,
                CodigoEstado = e.CodigoEstado,
                Pais = e.Pais
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetEstado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetEstado(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al estado con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var estado = await _estadoRepositorio.Obtener(
                    x => x.IdEstado == id,
                    incluir: query => query
                        .Include(e => e.Pais));

                if (estado == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new EstadoGetDto
                {
                    IdEstado = id,
                    NombreEstado = estado.NombreEstado,
                    CodigoEstado = estado.CodigoEstado,
                    Pais = estado.Pais,
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

        [HttpGet("BuscarEstadoPorNombre", Name = "GetNombreEstado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetNombreEstado([FromQuery] string nombre)
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

                IEnumerable<Estado> estados = await _estadoRepositorio.ObtenerTodos(
                    e => e.NombreEstado.Contains(nombre),
                    incluir: query => query
                    .Include(e => e.Pais));

                _response.Resultado = estados;
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
        public async Task<IActionResult> UpdateEstado(int id, [FromBody] EstadoUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdEstado)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.PaisId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar el estado",
                        "Es necesario hacer referencia a los campos de estado"
                    };
                    return BadRequest(_response);
                }

                var estadoExistente = await _estadoRepositorio.Obtener(ee => ee.IdEstado == id);
                if (estadoExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                estadoExistente.NombreEstado = updateDto.NombreEstado;
                estadoExistente.CodigoEstado = updateDto.CodigoEstado;
                estadoExistente.PaisId = updateDto.PaisId;

                await _estadoRepositorio.Actualizar(estadoExistente);
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
        public async Task<IActionResult> DeleteEnvio(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var estado = await _estadoRepositorio.Obtener(x => x.IdEstado == id);
                if (estado == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _estadoRepositorio.Remover(estado);
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
