namespace EcommWebApi.Repositorios
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        // Aqui es donde se trabajara con la base de datos
        private readonly DatosDbContext _context;
        internal DbSet<T> dbSet;

        public Repositorio(DatosDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public virtual async Task Crear(T entidad)
        {
            await dbSet.AddAsync(entidad);
            await GuardarCambios();
        }

        public async Task GuardarCambios()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true, Func<IQueryable<T>, IQueryable<T>>? incluir = null)
        {
            IQueryable<T> query = dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filtro != null)
                query = query.Where(filtro);

            if (incluir != null)
                query = incluir(query);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null, Func<IQueryable<T>, IQueryable<T>>? incluir = null)
        {
            IQueryable<T> query = dbSet;

            if (filtro != null)
                query = query.Where(filtro);

            if (incluir != null)
                query = incluir(query);

            return await query.ToListAsync();
        }

        public async Task Remover(T entidad)
        {
            dbSet.Remove(entidad);
            await GuardarCambios();
        }
    }
}