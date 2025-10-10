using VeterinaryClinic.Menus;
using VeterinaryClinic.Data;

class Program
{
    static async Task Main(string[] args)
    {
        await DataInitializer.InitializeAsync(); // 🔹 Carga todos los diccionarios
        await Login.ShowAsync();                // 🔹 Luego muestra el login
    }
}
