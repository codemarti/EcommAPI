namespace EcommWebApi.Repositorios
{
    // Decimos que la interfaz sera de tipo T para que pueda recibir cualquier tipo de entidad
    // O en otras palabras tener una interfaz generica
    public interface IRepositorio<T> where T : class
    {
        Task Crear(T entidad);
        Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null, Func<IQueryable<T>, IQueryable<T>>? incluir = null);
        Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true, Func<IQueryable<T>, IQueryable<T>>? incluir = null);
        Task Remover(T entidad);
        Task GuardarCambios();
    }
}
