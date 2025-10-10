using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;
using Helpers;

namespace VeterinaryClinic.Services
{
    public static class CustomerService
    {
        private static readonly CustomerRepository _repository = new CustomerRepository();

        // ✅ REGISTER CUSTOMER
        public static async Task RegisterCustomerAsync()
        {
            Console.WriteLine("--- 🧾 Register New Customer ---");

            string name = Validations.ValidateContent("Enter customer's name: ");
            int age = Validations.ValidateNumber("Enter customer's age: ");
            string address = Validations.ValidateContent("Enter customer's address: ");
            string phone = Validations.ValidateContent("Enter customer's phone: ");

            Customer newCustomer = new Customer(name, age, address, phone);

            // Register pets and associate them with the customer
            List<Guid> petsId = PetService.RegisterPet(newCustomer.Id);
            newCustomer.PetIds = petsId;

            // Save to Firebase
            string generatedId = await _repository.CrearAsync(newCustomer);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✅ Customer registered successfully!");
            Console.WriteLine($"🆔 Firebase ID: {generatedId}");
            Console.WriteLine($"👤 Name: {newCustomer.Name}");
            Console.WriteLine($"🐾 Pets linked: {newCustomer.PetIds.Count}");
            Console.ResetColor();
        }

        // ✅ LIST ALL CUSTOMERS
        public static async Task ListCustomersAsync()
        {
            Console.WriteLine("--- 📋 Customer List ---\n");

            var customersDict = await _repository.ObtenerTodosAsync();

            if (customersDict == null || customersDict.Count == 0)
            {
                Console.WriteLine("⚠️ No customers found.\n");
                return;
            }

            foreach (var entry in customersDict)
            {
                var id = entry.Key;
                var customer = entry.Value;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"🆔 Firebase ID : {id}");
                Console.ResetColor();

                Console.WriteLine($"👤 Name         : {customer.Name}");
                Console.WriteLine($"🎂 Age          : {customer.Age}");
                Console.WriteLine($"🏠 Address      : {customer.Address}");
                Console.WriteLine($"📞 Phone        : {customer.Phone}");

                if (customer.PetIds != null && customer.PetIds.Count > 0)
                {
                    Console.WriteLine($"🐾 Pets linked  : {customer.PetIds.Count}");
                }
                else
                {
                    Console.WriteLine($"🐾 Pets linked  : None");
                }

                Console.WriteLine(new string('-', 40));
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }
    }
}
