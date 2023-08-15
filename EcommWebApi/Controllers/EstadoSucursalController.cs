using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoSucursalController : ControllerBase
    {
        private readonly ILogger<EstadoSucursalController> _logger;
        private readonly IEstadoSucursalRepositorio _edoSucursalRepositorio;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public EstadoSucursalController(ILogger<EstadoSucursalController> logger
            , IEstadoSucursalRepositorio edoSucursalRepositorio
            , IMapper mapper)
        {
            _logger = logger;
            _edoSucursalRepositorio = edoSucursalRepositorio;
            _mapper = mapper;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateEdoSucursal([FromBody] EstadoSucursalCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _edoSucursalRepositorio.Obtener(v => v.NombreEdoSucursal.ToLower() == createDto.NombreEdoSucursal.ToLower()) != null)
                {
                    ModelState.AddModelError("nombreExiste", "El nombre ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                EstadoSucursal modelo = _mapper.Map<EstadoSucursal>(createDto);

                await _edoSucursalRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetEdoSucursal", new { id = modelo.IdEdoSucursal }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetEdoSucursales()
        {
            try
            {
                _logger.LogInformation("Obtener el estado de las sucursales");

                IEnumerable<EstadoSucursal> edoSucursalList = await _edoSucursalRepositorio.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<EstadoSucursalGetDto>>(edoSucursalList);
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

        [HttpGet("{id:int}", Name = "GetEdoSucursal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetEdoSucursal(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener estado de sucursal con el ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var edoSucursal = await _edoSucursalRepositorio.Obtener(x => x.IdEdoSucursal == id);
                if (edoSucursal == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<EstadoSucursalGetDto>(edoSucursal);
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

        [HttpGet("BuscarPorNombreEdoSucursal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> BuscarPorNombreEdoSucursal([FromQuery] string nombre)
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

                var edoSucursal = await _edoSucursalRepositorio.ObtenerTodos(x => x.NombreEdoSucursal == nombre);
                if (edoSucursal == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<EstadoSucursalGetDto>(edoSucursal);
                _response.StatusCode = HttpStatusCode.OK;

                _response.Resultado = edoSucursal;
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
        public async Task<IActionResult> UpdateEdoSucursal(int id, [FromBody] EstadoSucursalUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdEdoSucursal)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                EstadoSucursal modelo = _mapper.Map<EstadoSucursal>(updateDto);
                await _edoSucursalRepositorio.Actualizar(modelo);

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
        public async Task<IActionResult> DeleteEdoSucursal(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var edoSucursal = await _edoSucursalRepositorio.Obtener(x => x.IdEdoSucursal == id);
                if (edoSucursal == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _edoSucursalRepositorio.Remover(edoSucursal);
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
