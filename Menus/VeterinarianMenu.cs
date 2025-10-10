using VeterinaryClinic.Models;
using VeterinaryClinic.Services;

namespace VeterinaryClinic.Menus
{
    public static class VeterinarianMenu
    {
        public static async Task ShowAsync(User user)
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine($"🩺 Welcome {user.Name}, Veterinarian Menu");
                Console.WriteLine("==========================================");
                Console.WriteLine("1️⃣  View my consultations");
                Console.WriteLine("0️⃣  Exit");
                Console.WriteLine("==========================================");
                Console.Write("Choose an option: ");

                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await VeterinarianService.ShowConsultationsAsync(user.EntityId);
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("⚠️ Invalid option. Try again.");
                        await Task.Delay(1000);
                        break;
                }
            }
        }
    }
}
