using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetodoPagoController : ControllerBase
    {
        private readonly ILogger<MetodoPagoController> _logger;
        private readonly IMetodoPagoRepositorio _metodoPagoRepositorio;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public MetodoPagoController(ILogger<MetodoPagoController> logger
            , IMetodoPagoRepositorio metodoPagoRepositorio
            , IMapper mapper)
        {
            _logger = logger;
            _metodoPagoRepositorio = metodoPagoRepositorio;
            _mapper = mapper;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateMtdPago([FromBody] MetodoPagoCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _metodoPagoRepositorio.Obtener(v => v.NombreMetodoPago.ToLower() == createDto.NombreMetodoPago.ToLower()) != null)
                {
                    ModelState.AddModelError("nombreExiste", "El nombre ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                MetodoPago modelo = _mapper.Map<MetodoPago>(createDto);

                await _metodoPagoRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetMtdPago", new { id = modelo.IdMetodoPago }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetMtdPagos()
        {
            try
            {
                _logger.LogInformation("Obtener los metodos de pagos disponibles");

                IEnumerable<MetodoPago> mtdPagoList = await _metodoPagoRepositorio.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<MetodoPagoGetDto>>(mtdPagoList);
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

        [HttpGet("{id:int}", Name = "GetMtdPago")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetMtdPago(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener metodo de pago con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var mtdPago = await _metodoPagoRepositorio.Obtener(x => x.IdMetodoPago == id);
                if (mtdPago == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<MetodoPagoGetDto>(mtdPago);
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

        [HttpGet("BuscarPorNombreMtdPago")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> BuscarPorNombreMtdPago([FromQuery] string nombre)
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

                var mtdPago = await _metodoPagoRepositorio.ObtenerTodos(x => x.NombreMetodoPago == nombre);
                if (mtdPago == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<MetodoPagoGetDto>(mtdPago);
                _response.StatusCode = HttpStatusCode.OK;

                _response.Resultado = mtdPago;
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
        public async Task<IActionResult> UpdateMtdPago(int id, [FromBody] MetodoPagoUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdMetodoPago)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                MetodoPago modelo = _mapper.Map<MetodoPago>(updateDto);
                await _metodoPagoRepositorio.Actualizar(modelo);

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
        public async Task<IActionResult> DeleteMtdPago(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var mtdPago = await _metodoPagoRepositorio.Obtener(x => x.IdMetodoPago == id);
                if (mtdPago == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _metodoPagoRepositorio.Remover(mtdPago);
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
