using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalController : ControllerBase
    {
        private readonly ILogger<SucursalController> _logger;
        private readonly ISucursalRepositorio _sucursalRepositorio;
        private readonly ICadenaRepositorio _cadenaRepositorio;
        private readonly IDireccionRepositorio _direccionRepositorio;
        private readonly IEstadoSucursalRepositorio _edoSucursalRepositorio;
        private readonly IHorarioSucursalRepositorio _horarioRepositorio;
        private readonly IServiciosSucursalRepositorio _serviciosRepositorio;
        protected ApiResponse _response;

        public SucursalController(ILogger<SucursalController> logger
            , ISucursalRepositorio sucursalRepositorio
            , ICadenaRepositorio cadenaRepositorio
            , IDireccionRepositorio direccionRepositorio
            , IEstadoSucursalRepositorio edoSucursalRepositorio
            , IHorarioSucursalRepositorio horarioRepositorio
            , IServiciosSucursalRepositorio serviciosRepositorio)
        {
            _logger = logger;
            _sucursalRepositorio = sucursalRepositorio;
            _cadenaRepositorio = cadenaRepositorio;
            _direccionRepositorio = direccionRepositorio;
            _edoSucursalRepositorio = edoSucursalRepositorio;
            _horarioRepositorio = horarioRepositorio;
            _serviciosRepositorio = serviciosRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateSucursal([FromBody] SucursalCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _sucursalRepositorio.Obtener(s => s.RFCS.ToLower() == createDto.RFCS.ToLower()) != null)
                {
                    ModelState.AddModelError("rfcExiste", "Error este rfc ya existe");
                    return BadRequest(ModelState);
                }

                if (await _sucursalRepositorio.Obtener(s => s.NombreSucursal.ToLower() == createDto.NombreSucursal.ToLower()) != null)
                {
                    ModelState.AddModelError("nombreExiste", "Error este nombre ya existe");
                    return BadRequest(ModelState);
                }

                if (await _sucursalRepositorio.Obtener(s => s.TelSucursal == createDto.TelSucursal) != null)
                {
                    ModelState.AddModelError("telefonoExiste", "Error este telefono ya existe");
                    return BadRequest(ModelState);
                }

                if (await _sucursalRepositorio.Obtener(s => s.PPS.ToLower() == createDto.PPS.ToLower()) != null)
                {
                    ModelState.AddModelError("telefonoExiste", "Error este telefono ya existe");
                    return BadRequest(ModelState);
                }

                if (await _sucursalRepositorio.Obtener(s => s.TCS.ToLower() == createDto.TCS.ToLower()) != null)
                {
                    ModelState.AddModelError("telefonoExiste", "Error este telefono ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                if (createDto.CadenaId <= 0
                    || createDto.DireccionId <= 0
                    || createDto.EdoSucursalId <= 0
                    || createDto.ServicioId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion",
                        "Es necesario llenar a los campos que pertenecen a la sucursal"
                    };
                    return BadRequest(_response);
                }

                var cadena = await _cadenaRepositorio.Obtener(c => c.IdCadena == createDto.CadenaId,
                    incluir: query => query
                    .Include(c => c.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(c => c.TipoCadena)
                        .ThenInclude(tc => tc.ModeloNegocio));
                var direccion = await _direccionRepositorio.Obtener(d => d.IdDireccion == createDto.DireccionId,
                    incluir: query => query
                    .Include(d => d.Ciudad)
                        .ThenInclude(ci => ci.Estado)
                            .ThenInclude(e => e.Pais));
                var edoSucursal = await _edoSucursalRepositorio.Obtener(es => es.IdEdoSucursal == createDto.EdoSucursalId);
                var horario = await _horarioRepositorio.Obtener(hs => hs.IdHorario == createDto.HorarioId);
                var servicio = await _serviciosRepositorio.Obtener(sc => sc.IdServicio == createDto.ServicioId);

                Sucursal modelo = new()
                {
                    RFCS = createDto.RFCS,
                    NombreSucursal = createDto.NombreSucursal,
                    TelSucursal = createDto.TelSucursal,
                    FechaApertura = createDto.FechaApertura,
                    FechaCierre = createDto.FechaCierre,
                    ImagenSucursal = createDto.ImagenSucursal,
                    PPS = createDto.PPS,
                    TCS = createDto.TCS,
                    Detalles = createDto.Detalles,
                    Cadena = cadena,
                    Direccion = direccion,
                    EstadoSucursal = edoSucursal,
                    HorarioSucursal = horario,
                    ServiciosSucursal = servicio
                };

                await _sucursalRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetSucursal", new { id = modelo.IdSucursal }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetSucursales()
        {
            IEnumerable<Sucursal> sucursalList = await _sucursalRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(s => s.Cadena)
                    .ThenInclude(c => c.TipoCadena)
                        .ThenInclude(tp => tp.ModeloNegocio)
                .Include(s => s.Direccion)
                    .ThenInclude(d => d.Ciudad)
                        .ThenInclude(ci => ci.Estado)
                            .ThenInclude(e => e.Pais)
                .Include(s => s.EstadoSucursal)
                .Include(s=>s.HorarioSucursal)
                .Include(s => s.ServiciosSucursal));

            _response.Resultado = sucursalList.Select(s => new SucursalGetDto
            {
                IdSucursal = s.IdSucursal,
                RFCS = s.RFCS,
                NombreSucursal = s.NombreSucursal,
                TelSucursal = s.TelSucursal,
                FechaApertura = s.FechaApertura,
                FechaCierre = s.FechaCierre,
                ImagenSucursal = s.ImagenSucursal,
                PPS = s.PPS,
                TCS = s.TCS,
                Detalles = s.Detalles,
                Cadena = s.Cadena,
                Direccion = s.Direccion,
                EstadoSucursal = s.EstadoSucursal,
                Horario = s.HorarioSucursal,
                ServiciosSucursal = s.ServiciosSucursal
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetSucursal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetSucursal(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al sucursal con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var sucursal = await _sucursalRepositorio.Obtener(
                    x => x.IdSucursal == id,
                    incluir: query => query
                    .Include(s => s.Cadena)
                        .ThenInclude(c => c.TipoCadena)
                            .ThenInclude(tp => tp.ModeloNegocio)
                    .Include(s => s.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(s => s.EstadoSucursal)
                    .Include(s => s.HorarioSucursal)
                    .Include(s => s.ServiciosSucursal));

                if (sucursal == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new SucursalGetDto
                {
                    IdSucursal = id,
                    RFCS = sucursal.RFCS,
                    NombreSucursal = sucursal.NombreSucursal,
                    TelSucursal = sucursal.TelSucursal,
                    FechaApertura = sucursal.FechaApertura,
                    FechaCierre = sucursal.FechaCierre,
                    ImagenSucursal = sucursal.ImagenSucursal,
                    PPS = sucursal.PPS,
                    TCS = sucursal.TCS,
                    Detalles = sucursal.Detalles,
                    Cadena = sucursal.Cadena,
                    Direccion = sucursal.Direccion,
                    EstadoSucursal = sucursal.EstadoSucursal,
                    Horario = sucursal.HorarioSucursal,
                    ServiciosSucursal = sucursal.ServiciosSucursal
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

        [HttpGet("GetPorNombreSucursal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetPorNombreSucursal([FromQuery] string nombre)
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

                IEnumerable<Sucursal> sucursales = await _sucursalRepositorio.ObtenerTodos(
                    s => s.NombreSucursal.Contains(nombre),
                    incluir: query => query
                    .Include(s => s.Cadena)
                        .ThenInclude(c => c.TipoCadena)
                            .ThenInclude(tp => tp.ModeloNegocio)
                    .Include(s => s.Direccion)
                        .ThenInclude(d => d.Ciudad)
                            .ThenInclude(ci => ci.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(s => s.EstadoSucursal)
                    .Include(s => s.HorarioSucursal)
                    .Include(s => s.ServiciosSucursal));

                if (sucursales == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = sucursales;
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
        public async Task<IActionResult> UpdateSucursal(int id, [FromBody] SucursalUpdateDto updateDto)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.CadenaId <= 0
                    || updateDto.DireccionId <= 0
                    || updateDto.EdoSucursalId <= 0
                    || updateDto.ServicioId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar la sucursal",
                        "Es necesario hacer referencia a los campos de la sucursal"
                    };
                    return BadRequest(_response);
                }

                var sucursalExistente = await _sucursalRepositorio.Obtener(s => s.IdSucursal == id);
                if (sucursalExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                sucursalExistente.RFCS = updateDto.RFCS;
                sucursalExistente.NombreSucursal = updateDto.NombreSucursal;
                sucursalExistente.TelSucursal = updateDto.TelSucursal;
                sucursalExistente.FechaApertura = updateDto.FechaApertura;
                sucursalExistente.FechaCierre = updateDto.FechaCierre;
                sucursalExistente.ImagenSucursal = updateDto.ImagenSucursal;
                sucursalExistente.PPS = updateDto.PPS;
                sucursalExistente.TCS = updateDto.TCS;
                sucursalExistente.Detalles = updateDto.Detalles;
                sucursalExistente.DireccionId = updateDto.DireccionId;
                sucursalExistente.CadenaId = updateDto.CadenaId;
                sucursalExistente.EdoSucursalId = updateDto.EdoSucursalId;
                sucursalExistente.HorarioId = updateDto.HorarioId;
                sucursalExistente.ServicioId = updateDto.ServicioId;

                await _sucursalRepositorio.Actualizar(sucursalExistente);
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

                var sucursal = await _sucursalRepositorio.Obtener(x => x.IdSucursal == id);
                if (sucursal == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _sucursalRepositorio.Remover(sucursal);
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
