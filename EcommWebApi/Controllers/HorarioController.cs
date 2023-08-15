using Domain.Entidades.EFuertes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioController : ControllerBase
    {
        private readonly ILogger<HorarioController> _logger;
        private readonly IHorarioSucursalRepositorio _horarioRepositorio;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public HorarioController(ILogger<HorarioController> logger
            , IHorarioSucursalRepositorio horarioRepositorio
            , IMapper mapper)
        {
            _logger = logger;
            _horarioRepositorio = horarioRepositorio;
            _mapper = mapper;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateHorario([FromBody] HorarioSucursalCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (createDto is null)
                    return BadRequest();

                HorarioSucursal horario = _mapper.Map<HorarioSucursal>(createDto);

                await _horarioRepositorio.Crear(horario);
                _response.Resultado = horario;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetHorario", new { id = horario.IdHorario }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetHorarios()
        {
            try
            {
                _logger.LogInformation("Obtener los horarios");

                IEnumerable<HorarioSucursal> horarioList = await _horarioRepositorio.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<HorarioSucursalGetDto>>(horarioList);
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

        [HttpGet("{id:int}", Name = "GetHorario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetHorario(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener horario con el ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var horario = await _horarioRepositorio.Obtener(x => x.IdHorario == id);
                if (horario == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<HorarioSucursalGetDto>(horario);
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

        //[HttpGet("BuscarPorNombreMarca")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<ApiResponse>> BuscarPorNombreMarca([FromQuery] string nombre)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(nombre))
        //        {
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            _response.IsExistoso = false;
        //            _response.ErrorMessages = new List<string>() { "El parámetro 'nombre' es requerido." };
        //            return BadRequest(_response);
        //        }

        //        var marca = await _marcaRepositorio.ObtenerTodos(x => x.NombreMarca == nombre);
        //        if (marca == null)
        //        {
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            _response.IsExistoso = false;
        //            return NotFound(_response);
        //        }
        //        _response.Resultado = _mapper.Map<GeneroGetDto>(marca);
        //        _response.StatusCode = HttpStatusCode.OK;

        //        _response.Resultado = marca;
        //        _response.StatusCode = HttpStatusCode.OK;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.StatusCode = HttpStatusCode.InternalServerError;
        //        _response.IsExistoso = false;
        //        _response.ErrorMessages = new List<string>() { ex.Message };
        //        return StatusCode(StatusCodes.Status500InternalServerError, _response);
        //    }
        //}
        #endregion
        #region " APARTADO UPDATE "
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHorario(int id, [FromBody] HorarioSucursalUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdHorario)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                HorarioSucursal horario = _mapper.Map<HorarioSucursal>(updateDto);
                await _horarioRepositorio.Actualizar(horario);

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
        public async Task<IActionResult> DeleteHorario(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var horario = await _horarioRepositorio.Obtener(x => x.IdHorario == id);
                if (horario == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _horarioRepositorio.Remover(horario);
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
