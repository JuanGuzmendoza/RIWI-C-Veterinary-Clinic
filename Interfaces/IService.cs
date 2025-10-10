namespace VeterinaryClinic.Interfaces
{
    public interface IService<T>
    {
        Task RegisterAsync();   // Create
        Task ListAsync();       // Read all
        Task ShowAsync();       // Read one (optional display)
        Task UpdateAsync();     // Update
        Task DeleteAsync();     // Delete
    }
}
