using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;
using Helpers;
using VeterinaryClinic.Data;

namespace VeterinaryClinic.Services
{
    public static class CustomerService
    {
        private static readonly CustomerRepository _repository = new();

        // ✅ CREATE CUSTOMER
        public static async Task<Guid> RegisterAsync()
        {
            Console.WriteLine("--- 🧾 Register New Customer ---");

            string name = Validations.ValidateContent("Enter customer's name: ");
            int age = Validations.ValidateNumber("Enter customer's age: ");
            string address = Validations.ValidateContent("Enter customer's address: ");
            int phone = Validations.ValidateNumber("Enter customer's phone: ");

            Customer newCustomer = new(name, age, address, phone.ToString());

            // Register pets and associate them with the customer
            List<Guid> petsId = await PetService.RegisterAsync(newCustomer.Id);
            newCustomer.PetIds = petsId;

            // Save to Firebase
            string generatedId = await _repository.CrearAsync(newCustomer);

            // Guardar en memoria local
            DataStore.Customers[generatedId] = newCustomer;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✅ Customer registered successfully!");
            Console.WriteLine($"🆔 Firebase ID: {generatedId}");
            Console.WriteLine($"👤 Name: {newCustomer.Name}");
            Console.WriteLine($"🐾 Pets linked: {newCustomer.PetIds.Count}");
            Console.ResetColor();

            return newCustomer.Id; // 👈 devuelve el GUID para asociarlo al User
        }

        // ✅ READ ALL CUSTOMERS
        public static async Task ListAsync()
        {
            Console.WriteLine("--- 📋 Customer List ---\n");

            if (DataStore.Customers == null || DataStore.Customers.Count == 0)
            {
                // Si no hay datos en memoria, carga desde Firebase
                DataStore.Customers = await _repository.ObtenerTodosAsync();
            }

            if (DataStore.Customers.Count == 0)
            {
                Console.WriteLine("⚠️ No customers found.\n");
                return;
            }

            foreach (var entry in DataStore.Customers)
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

                Console.WriteLine($"🐾 Pets linked  : {(customer.PetIds?.Count ?? 0)}");
                Console.WriteLine(new string('-', 40));
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        // ✅ READ ONE CUSTOMER (by name)
        public static async Task ShowAsync()
        {
            string name = Validations.ValidateContent("Enter customer's name: ");

            var customerEntry = DataStore.Customers
                .FirstOrDefault(c => 
                    c.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (customerEntry.Value == null)
            {
                Console.WriteLine("❌ Customer not found.");
                return;
            }

            Customer.ShowInformation(customerEntry.Value);
        }

        // ✅ UPDATE CUSTOMER (by name)
        public static async Task UpdateAsync()
        {
            string name = Validations.ValidateContent("Enter customer's name to update: ");

            var customerEntry = DataStore.Customers
                .FirstOrDefault(c => 
                    c.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (customerEntry.Value == null)
            {
                Console.WriteLine("❌ Customer not found.");
                return;
            }

            string firebaseId = customerEntry.Key;
            var existing = customerEntry.Value;

            existing.Name = Validations.ValidateContent("Enter new name: ");
            existing.Age = Validations.ValidateNumber("Enter new age: ");
            existing.Address = Validations.ValidateContent("Enter new address: ");
            existing.Phone = Validations.ValidateContent("Enter new phone: ");

            await _repository.ActualizarAsync(firebaseId, existing);

            // Actualizar en memoria
            DataStore.Customers[firebaseId] = existing;

            Console.WriteLine("✅ Customer updated successfully!");
            Console.WriteLine("\nPress any key to return to the menu...");
           Console.ReadKey();

        }

        // ✅ DELETE CUSTOMER (by name)
        public static async Task DeleteAsync()
        {
            string name = Validations.ValidateContent("Enter customer's name to delete: ");

            var customerEntry = DataStore.Customers
                .FirstOrDefault(c => 
                    c.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (customerEntry.Value == null)
            {
                Console.WriteLine("❌ Customer not found.");
                return;
            }

            string firebaseId = customerEntry.Key;

            await _repository.EliminarAsync(firebaseId);
            DataStore.Customers.Remove(firebaseId);

            Console.WriteLine($"🗑️ Customer '{name}' deleted successfully!");
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();

        }
    }
}
