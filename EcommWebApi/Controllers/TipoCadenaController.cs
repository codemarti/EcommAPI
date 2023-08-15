using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoCadenaController : ControllerBase
    {
        private readonly ILogger<TipoCadenaController> _logger;
        private readonly ITipoCadenaRepositorio _tipoCadenaRepositorio;
        private readonly IModeloNegocioRepositorio _modeloRepositorio;
        protected ApiResponse _response;

        public TipoCadenaController(ILogger<TipoCadenaController> logger
            , ITipoCadenaRepositorio tipoCadenaRepositorio
            , IModeloNegocioRepositorio modeloRepositorio)
        {
            _logger = logger;
            _tipoCadenaRepositorio = tipoCadenaRepositorio;
            _modeloRepositorio = modeloRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateTipoCadena([FromBody] TipoCadenaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _tipoCadenaRepositorio.Obtener(s => s.NombreTipoCadena.ToLower() == createDto.NombreTipoCadena.ToLower()) != null)
                {
                    ModelState.AddModelError("nombreExiste", "Error este nombre ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                if (createDto.ModeloId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion",
                        "Es necesario llenar a los campos que pertenecen al tipo de la cadena"
                    };
                    return BadRequest(_response);
                }

                var modeloNegocio = await _modeloRepositorio.Obtener(m => m.IdModelo == createDto.ModeloId);

                TipoCadena modelo = new()
                {
                    NombreTipoCadena = createDto.NombreTipoCadena,
                    Descripcion = createDto.Descripcion,
                    ModeloNegocio = modeloNegocio
                };

                await _tipoCadenaRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetTipo", new { id = modelo.IdTipoCadena }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetTipos()
        {
            IEnumerable<TipoCadena> tipoCadenaList = await _tipoCadenaRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(tc => tc.ModeloNegocio));

            _response.Resultado = tipoCadenaList.Select(tc => new TipoCadenaGetDto
            {
                IdTipoCadena = tc.IdTipoCadena,
                NombreTipoCadena = tc.NombreTipoCadena,
                Descripcion = tc.Descripcion,
                ModeloNegocio = tc.ModeloNegocio
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetTipo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetTipo(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al tipo cadena con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var tipoCadena = await _tipoCadenaRepositorio.Obtener(
                    x => x.IdTipoCadena == id,
                    incluir: query => query
                    .Include(tc => tc.ModeloNegocio));

                if (tipoCadena == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new TipoCadenaGetDto
                {
                    IdTipoCadena = id,
                    NombreTipoCadena = tipoCadena.NombreTipoCadena,
                    Descripcion = tipoCadena.Descripcion,
                    ModeloNegocio = tipoCadena.ModeloNegocio
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

        [HttpGet("GetPorNombreTipo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetPorNombreTipo([FromQuery] string nombre)
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

                IEnumerable<TipoCadena> tiposCadena = await _tipoCadenaRepositorio.ObtenerTodos(
                    s => s.NombreTipoCadena.Contains(nombre),
                    incluir: query => query
                    .Include(tc => tc.ModeloNegocio));

                if (tiposCadena == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = tiposCadena;
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
        public async Task<IActionResult> UpdateTipoCadena(int id, [FromBody] TipoCadenaUpdateDto updateDto)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.ModeloId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar el tipo de cadena",
                        "Es necesario hacer referencia a los campos de la cadena"
                    };
                    return BadRequest(_response);
                }

                var tipoCadenaExistente = await _tipoCadenaRepositorio.Obtener(s => s.IdTipoCadena == id);
                if (tipoCadenaExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                tipoCadenaExistente.NombreTipoCadena = updateDto.NombreTipoCadena;
                tipoCadenaExistente.Descripcion = updateDto.Descripcion;
                tipoCadenaExistente.ModeloId = updateDto.ModeloId;

                await _tipoCadenaRepositorio.Actualizar(tipoCadenaExistente);
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
        public async Task<IActionResult> DeleteSucursal(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var tipoCadena = await _tipoCadenaRepositorio.Obtener(x => x.IdTipoCadena == id);
                if (tipoCadena == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _tipoCadenaRepositorio.Remover(tipoCadena);
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
