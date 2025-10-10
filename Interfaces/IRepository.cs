
namespace VeterinaryClinic.Interfaces
{
    public interface IRepository<T>
    {
        Task<string> CrearAsync(T entity);
        Task<Dictionary<string, T>> ObtenerTodosAsync();
        Task<T> ObtenerPorIdAsync(string id);
        Task ActualizarAsync(string id, T entity);
        Task EliminarAsync(string id);
    }
}
