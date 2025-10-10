using VeterinaryClinic.Models;
using VeterinaryClinic.Services;
using VeterinaryClinic.Data;
namespace VeterinaryClinic.Menus
{
    public static class CustomerMenu
    {
        public static async Task ShowAsync(User user)
        {
            bool exit = false;

            while (!exit)
            {
                await DataInitializer.InitializeAsync();
                Console.Clear();
                Console.WriteLine($"🐶 Welcome {user.Name}!");
                Console.WriteLine("========= Customer Menu =========");
                Console.WriteLine("1️⃣  Make a consultation");
                Console.WriteLine("2️⃣  View my pets");
                Console.WriteLine("0️⃣  Back to main menu");
                Console.WriteLine("=================================\n");
                Console.Write("Select an option: ");

                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await ConsultationService.CreateConsultationAsync(user.EntityId);
                        break;

                    case "2":
                        await PetService.ShowAsync(user.Id);
                        break;

                    case "0":
                        exit = true;
                        Console.WriteLine("👋 Returning to main menu...");
                        await Task.Delay(800);
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