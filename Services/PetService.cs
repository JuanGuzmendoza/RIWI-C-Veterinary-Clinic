using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;
using Helpers;
using VeterinaryClinic.Data;

namespace VeterinaryClinic.Services
{
    public static class PetService
    {
        private static readonly PetRepository _repository = new();

        // ✅ REGISTER PETS
        public static async Task<List<Guid>> RegisterAsync(Guid customerId)
        {
            List<Guid> petIds = new();
            bool addMore = true;

            while (addMore)
            {
                Console.WriteLine("--- 🐾 Register New Pet ---");

                string name = Validations.ValidateContent("Enter pet's name: ");
                string species = Validations.ValidateContent("Enter pet's species: ");
                string breed = Validations.ValidateContent("Enter pet's breed: ");
                string color = Validations.ValidateContent("Enter pet's color: ");

                Pet newPet = new(name, species, breed, color, customerId);
                string firebaseId = await _repository.CrearAsync(newPet);

                // 🔹 Guardar en memoria
                DataStore.Pets[firebaseId] = newPet;

                petIds.Add(newPet.Id);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ Pet registered successfully!\n");
                Console.ResetColor();

                string more = Validations.ValidateContent("Do you want to add another pet? (y/n): ");
                if (!more.Equals("y", StringComparison.OrdinalIgnoreCase))
                    addMore = false;
            }

            return petIds;
        }

        // ✅ LIST ALL PETS
        public static async Task ListAsync()
        {
            Console.WriteLine("--- 📋 Pet List ---");

            if (DataStore.Pets == null || DataStore.Pets.Count == 0)
            {
                // Cargar desde Firebase si la memoria está vacía
                DataStore.Pets = await _repository.ObtenerTodosAsync();
            }

            if (DataStore.Pets == null || DataStore.Pets.Count == 0)
            {
                Console.WriteLine("⚠️ No pets found.\n");
                return;
            }

            foreach (var entry in DataStore.Pets)
            {
                var pet = entry.Value;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"🆔 Firebase ID : {entry.Key}");
                Console.ResetColor();

                Console.WriteLine($"🐶 Name   : {pet.Name}");
                Console.WriteLine($"🐾 Species: {pet.Species}");
                Console.WriteLine($"🏷️ Breed  : {pet.Breed}");
                Console.WriteLine($"🎨 Color  : {pet.Color}");
                Console.WriteLine(new string('-', 40));
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        // ✅ SHOW ONE PET (by name)
        public static async Task ShowAsync()
        {
            string name = Validations.ValidateContent("Enter pet's name: ");

            var petEntry = DataStore.Pets
                .FirstOrDefault(p =>
                    p.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (petEntry.Value == null)
            {
                Console.WriteLine("❌ Pet not found.");
                return;
            }

            var pet = petEntry.Value;

            Console.WriteLine($"🐶 Name   : {pet.Name}");
            Console.WriteLine($"🐾 Species: {pet.Species}");
            Console.WriteLine($"🏷️ Breed  : {pet.Breed}");
            Console.WriteLine($"🎨 Color  : {pet.Color}");

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        // ✅ UPDATE PET (by name)
        public static async Task UpdateAsync()
        {
            string name = Validations.ValidateContent("Enter pet's name to update: ");

            var petEntry = DataStore.Pets
                .FirstOrDefault(p =>
                    p.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (petEntry.Value == null)
            {
                Console.WriteLine("❌ Pet not found.");
                return;
            }

            string firebaseId = petEntry.Key;
            var existing = petEntry.Value;

            existing.Name = Validations.ValidateContent("Enter new name: ");
            existing.Breed = Validations.ValidateContent("Enter new breed: ");
            existing.Color = Validations.ValidateContent("Enter new color: ");

            await _repository.ActualizarAsync(firebaseId, existing);

            // 🔹 Actualizar en memoria
            DataStore.Pets[firebaseId] = existing;

            Console.WriteLine("✅ Pet updated successfully!");
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        // ✅ DELETE PET (by name)
        public static async Task DeleteAsync()
        {
            string name = Validations.ValidateContent("Enter pet's name to delete: ");

            var petEntry = DataStore.Pets
                .FirstOrDefault(p =>
                    p.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (petEntry.Value == null)
            {
                Console.WriteLine("❌ Pet not found.");
                return;
            }

            string firebaseId = petEntry.Key;

            await _repository.EliminarAsync(firebaseId);
            DataStore.Pets.Remove(firebaseId);

            Console.WriteLine($"🗑️ Pet '{name}' deleted successfully!");
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }
    }
}
