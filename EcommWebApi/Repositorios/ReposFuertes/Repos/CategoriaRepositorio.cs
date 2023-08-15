namespace EcommWebApi.Repositorios.ReposFuertes.Repos;

public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
{
    private readonly DatosDbContext _context;

    public CategoriaRepositorio(DatosDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Categoria> Actualizar(Categoria entidad)
    {
        _context.Categorias.Update(entidad);
        await _context.SaveChangesAsync();

        return entidad;
    }
}
