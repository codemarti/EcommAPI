using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ILogger<ProductoController> _logger;
        private readonly IProductoRepositorio _productoRepositorio;
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IMarcaRepositorio _marcaRepositorio;
        protected ApiResponse _response;
        public ProductoController(ILogger<ProductoController> logger
            , IProductoRepositorio productoRepositorio
            , ICategoriaRepositorio categoriaRepositorio
            , IMarcaRepositorio marcaRepositorio)
        {
            _logger = logger;
            _productoRepositorio = productoRepositorio;
            _categoriaRepositorio = categoriaRepositorio;
            _marcaRepositorio = marcaRepositorio;
            _response = new();
        }

        #region " APARTADO CREATE "
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateProducto([FromBody] ProductoCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _productoRepositorio.Obtener(p => p.NombreProducto.ToLower() == createDto.NombreProducto.ToLower()) != null)
                {
                    ModelState.AddModelError("nombreExiste", "El nombre ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto is null)
                    return BadRequest();

                if (createDto.CategoriaId <= 0 || createDto.MarcaId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al crear el producto",
                        "Es necesario hacer referencia a marca o categoria"
                    };
                    return BadRequest(_response);
                }

                // Obtener la categoría y marca necesarias para el mapeo
                var categoria = await _categoriaRepositorio.Obtener(c => c.IdCategoria == createDto.CategoriaId);
                var marca = await _marcaRepositorio.Obtener(m => m.IdMarca == createDto.MarcaId);

                // Crear el objeto Producto y asignar los valores correspondientes
                Producto modelo = new()
                {
                    NombreProducto = createDto.NombreProducto,
                    FechaCreacion = DateTime.UtcNow,
                    Categoria = categoria,
                    Marca = marca
                };

                await _productoRepositorio.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetProducto", new { id = modelo.IdProducto }, _response);
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
        public async Task<ActionResult<ApiResponse>> GetProductos()
        {
            IEnumerable<Producto> productoList = await _productoRepositorio.ObtenerTodos();

            // Obtener las categorías y marcas necesarias para el mapeo
            IEnumerable<Categoria> categorias = await _categoriaRepositorio.ObtenerTodos();
            IEnumerable<Marca> marcas = await _marcaRepositorio.ObtenerTodos();

            // Mapear los productos a ProductoGetDto y asignar las categorías y marcas correspondientes
            _response.Resultado = productoList.Select(p => new ProductoGetDto
            {
                IdProducto = p.IdProducto,
                NombreProducto = p.NombreProducto,
                FechaCreacion = p.FechaCreacion,
                FechaActualizacion = p.FechaActualizacion,
                Categoria = categorias.FirstOrDefault(c => c.IdCategoria == p.CategoriaId),
                Marca = marcas.FirstOrDefault(m => m.IdMarca == p.MarcaId)
            }).ToList();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetProducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetProducto(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener producto con ID: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var producto = await _productoRepositorio.Obtener(x => x.IdProducto == id);
                if (producto == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                // Obtener la categoría y marca necesarias para el mapeo
                var categoria = await _categoriaRepositorio.Obtener(c => c.IdCategoria == producto.CategoriaId);
                var marca = await _marcaRepositorio.Obtener(m => m.IdMarca == producto.MarcaId);

                _response.Resultado = new ProductoGetDto
                {
                    IdProducto = producto.IdProducto,
                    NombreProducto = producto.NombreProducto,
                    FechaCreacion = producto.FechaCreacion,
                    FechaActualizacion = producto.FechaActualizacion,
                    Categoria = categoria,
                    Marca = marca
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

        [HttpGet("GetPorNombreProductoOCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> BuscarProductosPorNombreOCategoria([FromQuery] string nombre)
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

                // Obtener los productos por nombre
                IEnumerable<Producto> productos = await _productoRepositorio.ObtenerTodos(
                    p => p.NombreProducto.Contains(nombre)
                    || p.Categoria.NombreCategoria.Contains(nombre),
                    incluir: query => query
                    .Include(p => p.Categoria)
                    .Include(p => p.Marca));

                if (productos == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = productos;
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

        [HttpGet("GetPorRegistros", Name = "GetPaginado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetPaginado([FromQuery] int? pagina, int registros = 1)
        {
            try
            {
                if (registros == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    _response.ErrorMessages = new List<string>()
                    {
                        "Error de peticion",
                        "No se pueden mostrar 0 registros"
                    };
                    return StatusCode(StatusCodes.Status400BadRequest, _response);
                }
                int _pagina = pagina ?? 1;

                // Obtener los productos con las referencias de categorías y marcas cargadas
                IEnumerable<Producto> productoList = await _productoRepositorio
                    .ObtenerTodos(incluir: query => query.Include(p => p.Categoria).Include(p => p.Marca));

                // Creo mi total de registros dependiente de la línea anterior
                decimal totalRegistros = productoList.Count();

                int totalPaginas = Convert.ToInt32(Math.Ceiling(totalRegistros / registros));

                var productos = productoList.Skip((_pagina - 1) * registros).Take(registros).ToList();

                _response.Resultado = new
                {
                    paginas = totalPaginas,
                    registros = productos,
                    currentPagina = _pagina
                };

                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string> { ex.Message };
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
        public async Task<IActionResult> UpdateProducto(int id, [FromBody] ProductoUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.IdProducto)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updateDto.CategoriaId <= 0 || updateDto.MarcaId <= 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>
                    {
                        "Error de peticion al actualizar un producto",
                        "Es necesario hacer referencia a marca o categoria"
                    };
                    return BadRequest(_response);
                }

                // Obtener el producto existente
                var productoExistente = await _productoRepositorio.Obtener(p => p.IdProducto == id);
                if (productoExistente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                // Obtener la categoría y marca necesarias para el mapeo
                var categoria = await _categoriaRepositorio.Obtener(c => c.IdCategoria == updateDto.CategoriaId);
                var marca = await _marcaRepositorio.Obtener(m => m.IdMarca == updateDto.MarcaId);

                // Actualizar las propiedades del producto existente con los valores del DTO
                productoExistente.NombreProducto = updateDto.NombreProducto;
                productoExistente.Categoria = categoria;
                productoExistente.Marca = marca;

                await _productoRepositorio.Actualizar(productoExistente);

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
        public async Task<IActionResult> DeleteProducto(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var producto = await _productoRepositorio.Obtener(x => x.IdProducto == id);
                if (producto == null)
                {
                    _response.IsExistoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _productoRepositorio.Remover(producto);
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
