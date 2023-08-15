using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactoEmpleadoController : ControllerBase
    {
        private readonly ILogger<ContactoEmpleadoController> _logger;
        private readonly IContactoEmpleadoRepositorio _contactoRepositorio;
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        protected ApiResponse _response;

        public ContactoEmpleadoController(ILogger<ContactoEmpleadoController> logger
            , IContactoEmpleadoRepositorio contactoRepositorio
            , IEmpleadoRepositorio empleadoRepositorio)
        {
            _logger = logger;
            _contactoRepositorio = contactoRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateContacto([FromBody] ContactoEmpleadoCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (createDto is null)
                    return BadRequest();

                if (createDto.EmpleadoId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Error de peticion al hacer referencia a un empleado" };
                    return BadRequest(_response);
                }

                var empleado = await _empleadoRepositorio.Obtener(c => c.IdEmpleado == createDto.EmpleadoId,
                    incluir: query => query
                    .Include(e => e.Direccion)
                        .ThenInclude(e => e.Ciudad)
                            .ThenInclude(e => e.Estado)
                                .ThenInclude(e => e.Pais)
                    .Include(e => e.EstadoEmpleado)
                    .Include(e => e.Genero)
                    .Include(e => e.Rol));

                ContactoEmpleado contacto = new()
                {
                    NombreContacto = createDto.NombreContacto,
                    Telefono = createDto.Telefono,
                    Parentesco = createDto.Parentesco,
                    Empleado = empleado
                };

                await _contactoRepositorio.Crear(contacto);
                _response.Resultado = contacto;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetContacto", new { id = contacto.IdContacto }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetContactos()
        {
            IEnumerable<ContactoEmpleado> contactoList = await _contactoRepositorio.ObtenerTodos(
                incluir: query => query
                .Include(c => c.Empleado)
                    .ThenInclude(e => e.Direccion)
                        .ThenInclude(e => e.Ciudad)
                            .ThenInclude(e => e.Estado)
                                .ThenInclude(e => e.Pais)
                .Include(c => c.Empleado)
                    .ThenInclude(e => e.EstadoEmpleado)
                .Include(c => c.Empleado)
                    .ThenInclude(e => e.Genero)
                .Include(c => c.Empleado)
                    .ThenInclude(e => e.Rol));

            _response.Resultado = contactoList.Select(c => new ContactoEmpleadoGetDto
            {
                IdContacto = c.IdContacto,
                NombreContacto = c.NombreContacto,
                Telefono = c.Telefono,
                Parentesco = c.Parentesco,
                Empleado = c.Empleado
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetContacto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetContacto(int id)
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

                var contacto = await _contactoRepositorio.Obtener(
                    x => x.IdContacto == id,
                    incluir: query => query
                    .Include(c => c.Empleado)
                        .ThenInclude(e => e.Direccion)
                            .ThenInclude(e => e.Ciudad)
                                .ThenInclude(e => e.Estado)
                                    .ThenInclude(e => e.Pais)
                    .Include(c => c.Empleado)
                        .ThenInclude(e => e.EstadoEmpleado)
                    .Include(c => c.Empleado)
                        .ThenInclude(e => e.Genero)
                    .Include(c => c.Empleado)
                        .ThenInclude(e => e.Rol));

                if (contacto == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = new ContactoEmpleadoGetDto
                {
                    IdContacto = contacto.IdContacto,
                    NombreContacto = contacto.NombreContacto,
                    Telefono = contacto.Telefono,
                    Parentesco = contacto.Parentesco,
                    Empleado = contacto.Empleado
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

        [HttpGet("GetPorDataContacto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetPorDataContacto([FromQuery] string dataContacto)
        {
            try
            {
                if (string.IsNullOrEmpty(dataContacto))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>() { "El parámetro de 'datos' es requerido." };
                    return BadRequest(_response);
                }

                IEnumerable<ContactoEmpleado> contactos = await _contactoRepositorio.ObtenerTodos(
                    c => c.NombreContacto.Contains(dataContacto)
                    || c.Telefono.Contains(dataContacto)
                    || (c.Empleado.NombreEmpleado + " " + c.Empleado.ApellidoEmpleado).Contains(dataContacto),
                    incluir: query => query
                    .Include(c => c.Empleado)
                        .ThenInclude(e => e.Direccion)
                            .ThenInclude(e => e.Ciudad)
                                .ThenInclude(e => e.Estado)
                                    .ThenInclude(e => e.Pais)
                    .Include(c => c.Empleado)
                        .ThenInclude(e => e.EstadoEmpleado)
                    .Include(c => c.Empleado)
                        .ThenInclude(e => e.Genero)
                    .Include(c => c.Empleado)
                        .ThenInclude(e => e.Rol));

                if (contactos == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = contactos;
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
        public async Task<IActionResult> UpdateContacto(int id, [FromBody] ContactoEmpleadoUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdContacto)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.EmpleadoId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar el contacto",
                        "Es necesario hacer a los campos que contiene el contacto"
                    };
                    return BadRequest(_response);
                }

                var contactoExistente = await _contactoRepositorio.Obtener(d => d.IdContacto == id);
                if (contactoExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                contactoExistente.NombreContacto = updateDto.NombreContacto;
                contactoExistente.Telefono = updateDto.Telefono;
                contactoExistente.Parentesco = updateDto.Parentesco;
                contactoExistente.EmpleadoId = updateDto.EmpleadoId;

                await _contactoRepositorio.Actualizar(contactoExistente);
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

                var contacto = await _contactoRepositorio.Obtener(x => x.IdContacto == id);
                if (contacto == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _contactoRepositorio.Remover(contacto);
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
