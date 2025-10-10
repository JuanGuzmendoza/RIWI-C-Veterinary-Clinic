using VeterinaryClinic.Services;

class Program
{
    static async Task Main()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("🐾 Veterinary Clinic - Customer Management");
            Console.WriteLine("==========================================");
            Console.WriteLine("1️⃣  Register new customer");
            Console.WriteLine("2️⃣  List all customers");
            Console.WriteLine("0️⃣  Exit");
            Console.Write("\nSelect an option: ");

            string? option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    await CustomerService.RegisterCustomerAsync();
                    break;

                case "2":
                    await CustomerService.ListCustomersAsync();
                    break;

                case "0":
                    exit = true;
                    Console.WriteLine("👋 Goodbye!");
                    break;

                default:
                    Console.WriteLine("⚠️ Invalid option. Try again.");
                    break;
            }

            if (!exit)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
