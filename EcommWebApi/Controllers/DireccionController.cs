using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DireccionController : ControllerBase
    {
        private readonly ILogger<DireccionController> _logger;
        private readonly IDireccionRepositorio _direccionRepositorio;
        private readonly ICiudadRepositorio _ciudadRepositorio;
        protected ApiResponse _response;

        public DireccionController(ILogger<DireccionController> logger
            , IDireccionRepositorio direccionRepositorio
            , ICiudadRepositorio ciudadRepositorio)
        {
            _logger = logger;
            _direccionRepositorio = direccionRepositorio;
            _ciudadRepositorio = ciudadRepositorio;
            _response= new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateDireccion([FromBody] DireccionCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (createDto is null)
                    return BadRequest();

                if (createDto.CiudadId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Error de peticion al hacer referencia a una ciudad" };
                    return BadRequest(_response);
                }

                var ciudad = await _ciudadRepositorio.Obtener(c => c.IdCiudad == createDto.CiudadId,
                    incluir: query => query
                    .Include(c => c.Estado)
                        .ThenInclude(e => e.Pais));

                Direccion modelo = new()
                {
                    CP= createDto.CP,
                    Calle1= createDto.Calle1,
                    Calle2= createDto.Calle2,
                    NumExt= createDto.NumExt,
                    Detalles= createDto.Detalles,
                    Ciudad = ciudad
                };

                await _direccionRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetDireccion", new { id = modelo.IdDireccion }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetDirecciones()
        {
            IEnumerable<Direccion> direccionList = await _direccionRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(d => d.Ciudad)
                    .ThenInclude(c => c.Estado)
                        .ThenInclude(e => e.Pais)
            );

            _response.Resultado = direccionList.Select(c => new DireccionGetDto
            {
                IdDireccion = c.IdDireccion,
                CP = c.CP,
                Calle1 = c.Calle1,
                Calle2 = c.Calle2,
                NumExt = c.NumExt,
                Detalles = c.Detalles,
                Ciudad = c.Ciudad
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetDireccion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetDireccion(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener direccion con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var direccion = await _direccionRepositorio.Obtener(
                    x => x.IdDireccion == id,
                    incluir: query => query
                        .Include(d => d.Ciudad)
                            .ThenInclude(c => c.Estado)
                                .ThenInclude(e => e.Pais)
                );

                if (direccion == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new DireccionGetDto
                {
                    IdDireccion = id,
                    CP = direccion.CP,
                    Calle1 = direccion.Calle1,
                    Calle2 = direccion.Calle2,
                    NumExt = direccion.NumExt,
                    Detalles = direccion.Detalles,
                    Ciudad = direccion.Ciudad
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
        #endregion
        #region " APARTADO UPDATE "
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDireccion(int id, [FromBody] DireccionUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdDireccion)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.CiudadId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar la direccion",
                        "Es necesario hacer a los campos que contiene direccion"
                    };
                    return BadRequest(_response);
                }

                var direccionExistente = await _direccionRepositorio.Obtener(d => d.IdDireccion == id);
                if (direccionExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                direccionExistente.CP = updateDto.CP;
                direccionExistente.Calle1 = updateDto.Calle1;
                direccionExistente.Calle2 = updateDto.Calle2;
                direccionExistente.NumExt = updateDto.NumExt;
                direccionExistente.Detalles = updateDto.Detalles;
                direccionExistente.CiudadId = updateDto.CiudadId;

                await _direccionRepositorio.Actualizar(direccionExistente);
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
        public async Task<IActionResult> DeleteDireccion(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var direccion = await _direccionRepositorio.Obtener(x => x.IdDireccion == id);
                if (direccion == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _direccionRepositorio.Remover(direccion);
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
