using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        private readonly ILogger<PagoController> _logger;
        private readonly IRepositorio<Pago> _pagoRepositorio;
        private readonly IMetodoPagoRepositorio _mtdPagoRepositorio;
        protected ApiResponse _response;

        public PagoController(ILogger<PagoController> logger
            , IRepositorio<Pago> pagoRepositorio
            , IMetodoPagoRepositorio mtdPagoRepositorio)
        {
            _logger = logger;
            _pagoRepositorio = pagoRepositorio;
            _mtdPagoRepositorio = mtdPagoRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreatePago([FromBody] PagoCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _pagoRepositorio.Obtener(e => e.NumeroTarjeta == createDto.NumeroTarjeta) != null)
                {
                    ModelState.AddModelError("tarjetaExiste", "Error este numero de tarjeta ya existe");
                    return BadRequest(ModelState);
                }

                if (await _pagoRepositorio.Obtener(e => e.NombreTitular.ToLower() == createDto.NombreTitular.ToLower()) != null)
                {
                    ModelState.AddModelError("nombreExiste", "Error este nombre de titular de tarjeta ya existe");
                    return BadRequest(ModelState);
                }

                if (await _pagoRepositorio.Obtener(e => e.FechaExpiracion.ToString() == createDto.FechaExpiracion.ToString()) != null)
                {
                    ModelState.AddModelError("fechaExiste", "Error esta fecha de expiracion de tarjeta ya existe");
                    return BadRequest(ModelState);
                }

                if (await _pagoRepositorio.Obtener(e => e.CVV == createDto.CVV) != null)
                {
                    ModelState.AddModelError("codigoExiste", "Error este codigo de tarjeta ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                if (createDto.MetodoPagoId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion",
                        "Es necesario llenar a los campos que pertenecen a la tarjeta"
                    };
                    return BadRequest(_response);
                }

                var mtdPago = await _mtdPagoRepositorio.Obtener(mp => mp.IdMetodoPago == createDto.MetodoPagoId);

                Pago modelo = new()
                {
                    NombreTitular = createDto.NombreTitular,
                    NumeroTarjeta = createDto.NumeroTarjeta,
                    CVV = createDto.CVV,
                    FechaExpiracion = createDto.FechaExpiracion,
                    FechaCreacion = DateTime.UtcNow,
                    MetodoPago = mtdPago
                };

                await _pagoRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetTarjeta", new { referencia = modelo.IdPago }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetTarjetas()
        {
            IEnumerable<Pago> pagoList = await _pagoRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(p => p.MetodoPago));

            _response.Resultado = pagoList.Select(p => new PagoGetDto
            {
                IdPago = p.IdPago,
                NombreTitular = p.NombreTitular,
                NumeroTarjeta = p.NumeroTarjeta,
                FechaExpiracion = p.FechaExpiracion,
                CVV = p.CVV,
                MetodoPago = p.MetodoPago
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetTarjeta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetTarjeta(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al tarjeta con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var pago = await _pagoRepositorio.Obtener(
                    x => x.IdPago == id,
                    incluir: query => query
                        .Include(p => p.MetodoPago));

                if (pago == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new PagoGetDto
                {
                    IdPago = id,
                    NombreTitular = pago.NombreTitular,
                    NumeroTarjeta = pago.NumeroTarjeta,
                    FechaExpiracion = pago.FechaExpiracion,
                    CVV = pago.CVV,
                    MetodoPago = pago.MetodoPago
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
        //[HttpGet("BuscarEstadoPorNombre", Name = "GetNombreEstado")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<ApiResponse>> GetNombreEstado([FromQuery] string nombre)
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

        //        IEnumerable<Estado> estados = await _estadoRepositorio.ObtenerPorNombre(
        //            e => e.NombreEstado.Contains(nombre),
        //            incluir: query => query
        //            .Include(e => e.Pais));

        //        _response.Resultado = estados;
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
        #region " APARTADO DELETE "
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePago(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var pago = await _pagoRepositorio.Obtener(x => x.IdPago == id);
                if (pago == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _pagoRepositorio.Remover(pago);
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
