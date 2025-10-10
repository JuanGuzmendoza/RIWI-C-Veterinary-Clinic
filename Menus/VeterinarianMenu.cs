using VeterinaryClinic.Models;
namespace VeterinaryClinic.Menus
{
    public static class VeterinarianMenu
    {
        public static async Task ShowAsync(User user)
        {
            Console.WriteLine($"🩺 Welcome {user.Name}, Veterinarian Menu");
            await Task.Delay(1000);
        }
    }
}
