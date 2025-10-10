using System;
using System.Threading.Tasks;
using VeterinaryClinic.Data;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Menus
{
    public static class Login
    {
        public static async Task ShowAsync()
        {
            Console.Title = "🔐 Veterinary Clinic - Login";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("====================================");
            Console.WriteLine("       🏥 VETERINARY CLINIC 🏥      ");
            Console.WriteLine("====================================");
            Console.ResetColor();

            Console.Write("\n👤 Username: ");
            string? username = Console.ReadLine();

            Console.Write("🔒 Password: ");
            string? password = ReadPassword();

            // Validación directa para administrador
            if (username == "admin" && password == "123")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✅ Welcome Admin!");
                Console.ResetColor();
                await Task.Delay(1000);
                Console.Clear();
                await AdminMenu.ShowAsync();
                return;
            }

            // Validación desde los usuarios guardados
            if (ValidateCredentials(username, password, out User? user))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✅ Welcome {user.Name}! Role: {user.Role}");
                Console.ResetColor();
                await Task.Delay(1000);
                Console.Clear();

                switch (user.Role.ToLower())
                {
                    case "customer":
                        await CustomerMenu.ShowAsync(user);
                        break;
                    case "veterinarian":
                        await VeterinarianMenu.ShowAsync(user);
                        break;
                    default:
                        Console.WriteLine("⚠️ Unknown role. Access denied.");
                        break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Incorrect username or password.");
                Console.ResetColor();
                await Task.Delay(1500);
                Console.Clear();
                await ShowAsync(); // volver a intentar
            }
        }

        private static bool ValidateCredentials(string? username, string? password, out User? user)
        {
            user = null;
            foreach (var u in DataStore.Users.Values)
            {
                if (u.Username == username && u.Password == password)
                {
                    user = u;
                    return true;
                }
            }
            return false;
        }

        private static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password;
        }
    }
}
