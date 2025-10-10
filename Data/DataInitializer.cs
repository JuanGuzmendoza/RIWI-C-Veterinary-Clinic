using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Data
{
    public static class DataInitializer
    {
        public static async Task InitializeAsync()
        {
            Console.WriteLine("⏳ Cargando datos desde Firebase...\n");

            var customerRepo = new CustomerRepository();
            var petRepo = new PetRepository();
            var userRepo = new UserRepository();

            // 🔹 Cargar todo lo que haya en Firebase
            DataStore.Customers = await customerRepo.ObtenerTodosAsync() ?? new();
            DataStore.Pets  = await petRepo.ObtenerTodosAsync() ?? new();
            DataStore.Users     = await userRepo.ObtenerTodosAsync() ?? new();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✅ Datos cargados correctamente desde Firebase.\n");
            Console.ResetColor();
        }
    }
}
