using System;
using System.Threading.Tasks;
using VeterinaryClinic.Services;

namespace VeterinaryClinic.Menus
{
    public static class AdminMenu
    {
        public static async Task ShowAsync()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("====================================");
            Console.WriteLine("        ⚙️ ADMINISTRATOR MENU        ");
            Console.WriteLine("====================================");
            Console.ResetColor();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1️⃣  Manage Customers");
                Console.WriteLine("2️⃣  Manage Pets");
                Console.WriteLine("3️⃣  Manage Veterinarians");
                Console.WriteLine("4️⃣  Manage Users");
                Console.WriteLine("0️⃣  Exit");
                Console.Write("👉 Option: ");
                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await ShowCustomerMenuAsync();
                        break;
                    case "2":
                        await ShowPetMenuAsync();
                        break;
                    case "3":
                        await ShowVeterinarianMenuAsync();
                        break;
                    case "4":
                        await ShowUserMenuAsync();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Invalid option. Please try again.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        // 👥 Customer CRUD Menu
        private static async Task ShowCustomerMenuAsync()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\n📋 CUSTOMER MANAGEMENT");
                Console.WriteLine("1️⃣ List customers");
                Console.WriteLine("3️⃣ Edit customer");
                Console.WriteLine("4️⃣ Delete customer");
                Console.WriteLine("0️⃣ Back");
                Console.Write("👉 Option: ");
                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1": await CustomerService.ListAsync(); break;
                    case "3": await CustomerService.UpdateAsync(); break;
                    case "4": await CustomerService.DeleteAsync(); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("❌ Invalid option."); break;
                }
            }
        }

        // 🐾 Pet CRUD Menu
        private static async Task ShowPetMenuAsync()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\n🐶 PET MANAGEMENT");
                Console.WriteLine("1️⃣ List pets");
                Console.WriteLine("3️⃣ Edit pet");
                Console.WriteLine("4️⃣ Delete pet");
                Console.WriteLine("0️⃣ Back");
                Console.Write("👉 Option: ");
                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1": await PetService.ListAsync(); break;
                    case "3": await PetService.UpdateAsync(); break;
                    case "4": await PetService.DeleteAsync(); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("❌ Invalid option."); break;
                }
            }
        }

        // 🩺 Veterinarian CRUD Menu
        private static async Task ShowVeterinarianMenuAsync()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\n🩺 VETERINARIAN MANAGEMENT");
                Console.WriteLine("1️⃣ List veterinarians");
                Console.WriteLine("3️⃣ Edit veterinarian");
                Console.WriteLine("4️⃣ Delete veterinarian");
                Console.WriteLine("0️⃣ Back");
                Console.Write("👉 Option: ");
                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1": await VeterinarianService.ListAsync(); break;
                    case "3": await VeterinarianService.UpdateAsync(); break;
                    case "4": await VeterinarianService.DeleteAsync(); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("❌ Invalid option."); break;
                }
            }
        }

        // 👤 User CRUD Menu
        private static async Task ShowUserMenuAsync()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\n👤 USER MANAGEMENT");
                Console.WriteLine("1️⃣ List users");
                Console.WriteLine("2️⃣ Add user");
                Console.WriteLine("3️⃣ Edit user");
                Console.WriteLine("4️⃣ Delete user");
                Console.WriteLine("0️⃣ Back");
                Console.Write("👉 Option: ");
                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1": await UserService.ListAsync(); break;
                    case "2": await UserService.RegisterAsync(); break;
                    case "3": await UserService.UpdateAsync(); break;
                    case "4": await UserService.DeleteAsync(); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("❌ Invalid option."); break;
                }
            }
        }
    }
}
