using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CiudadController : ControllerBase
    {
        private readonly ILogger<CiudadController> _logger;
        private readonly ICiudadRepositorio _ciudadRepositorio;
        private readonly IEstadoRepositorio _estadoRepositorio;
        protected ApiResponse _response;

        public CiudadController(ILogger<CiudadController> logger
            , ICiudadRepositorio ciudadRepositorio
            , IEstadoRepositorio estadoRepositorio)
        {
            _logger = logger;
            _ciudadRepositorio = ciudadRepositorio;
            _estadoRepositorio = estadoRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateCiudad([FromBody] CiudadCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _ciudadRepositorio.Obtener(p => p.NombreCiudad.ToLower() == createDto.NombreCiudad.ToLower()) != null)
                {
                    ModelState.AddModelError("nombreExiste", "El nombre ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                if (createDto.EstadoId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion",
                        "Es necesario hacer referencia a los campos de ciudades"
                    };
                    return BadRequest(_response);
                }

                var estado = await _estadoRepositorio.Obtener(c => c.IdEstado == createDto.EstadoId,
                    incluir: query => query
                    .Include(e => e.Pais));

                Ciudad modelo = new()
                {
                    NombreCiudad = createDto.NombreCiudad,
                    Estado = estado,
                };

                await _ciudadRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetCiudad", new { id = modelo.IdCiudad }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetCiudades()
        {
            IEnumerable<Ciudad> ciudadList = await _ciudadRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(c => c.Estado)
                    .ThenInclude(e => e.Pais)
            );

            _response.Resultado = ciudadList.Select(c => new CiudadGetDto
            {
                IdCiudad = c.IdCiudad,
                NombreCiudad = c.NombreCiudad,
                Estado = c.Estado
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetCiudad")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetCiudad(int id)
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

                var ciudad = await _ciudadRepositorio.Obtener(
                    x => x.IdCiudad == id,
                    incluir: query => query
                        .Include(c => c.Estado)
                            .ThenInclude(e => e.Pais)
                );

                if (ciudad == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new CiudadGetDto
                {
                    IdCiudad = id,
                    NombreCiudad = ciudad.NombreCiudad,
                    Estado = ciudad.Estado,
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

        [HttpGet("GetPorNombreCiudad")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> BuscarCiudadesPorNombre([FromQuery] string nombre)
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

                IEnumerable<Ciudad> ciudades = await _ciudadRepositorio.ObtenerTodos(
                    c => c.NombreCiudad.Contains(nombre),
                    incluir: query => query
                        .Include(ci => ci.Estado)
                            .ThenInclude(e => e.Pais));

                if (ciudades.IsNullOrEmpty())
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = ciudades;
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
        public async Task<IActionResult> UpdateCiudad(int id, [FromBody] CiudadUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdCiudad)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.EstadoId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar la ciudad",
                        "Es necesario hacer referencia al estado o departamento de la ciudad"
                    };
                    return BadRequest(_response);
                }

                var ciudadExistente = await _ciudadRepositorio.Obtener(cp => cp.IdCiudad == id);
                if (ciudadExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                ciudadExistente.NombreCiudad = updateDto.NombreCiudad;
                ciudadExistente.EstadoId = updateDto.EstadoId;

                await _ciudadRepositorio.Actualizar(ciudadExistente);
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
        public async Task<IActionResult> DeleteCiudad(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var ciudad = await _ciudadRepositorio.Obtener(x => x.IdCiudad == id);
                if (ciudad == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _ciudadRepositorio.Remover(ciudad);
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
