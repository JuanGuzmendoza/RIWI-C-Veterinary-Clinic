using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;
using Helpers;

namespace VeterinaryClinic.Services
{
    public static class VeterinarianService
    {
        private static readonly VeterinarianRepository _repository = new();

        // ✅ CREATE VETERINARIAN
 public static async Task<Guid> RegisterAsync()
{
    Console.WriteLine("--- 🩺 Register New Veterinarian ---");

    string name = Validations.ValidateContent("Enter veterinarian's name: ");
    int age = Validations.ValidateNumber("Enter veterinarian's age: ");
    string address = Validations.ValidateContent("Enter veterinarian's address: ");
    string phone = Validations.ValidateContent("Enter veterinarian's phone: ");
    string specialization = Validations.ValidateContent("Enter veterinarian's specialization: ");

    Veterinarian newVet = new(name, age, address, phone, specialization);
    newVet.ConsultationIds = new List<Guid>();

    string generatedId = await _repository.CrearAsync(newVet);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\n✅ Veterinarian registered successfully!");
    Console.WriteLine($"🆔 Firebase ID: {generatedId}");
    Console.WriteLine($"👨‍⚕️ Name: {newVet.Name}");
    Console.WriteLine($"💼 Specialization: {newVet.Specialization}");
    Console.ResetColor();

    return newVet.Id; // 👈 devuelve el GUID
}

        // ✅ READ ALL VETERINARIANS
        public static async Task ListAsync()
        {
            Console.WriteLine("--- 📋 Veterinarian List ---\n");

            var vetsDict = await _repository.ObtenerTodosAsync();

            if (vetsDict == null || vetsDict.Count == 0)
            {
                Console.WriteLine("⚠️ No veterinarians found.\n");
                return;
            }

            foreach (var entry in vetsDict)
            {
                var id = entry.Key;
                var vet = entry.Value;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"🆔 Firebase ID : {id}");
                Console.ResetColor();

                Console.WriteLine($"👨‍⚕️ Name           : {vet.Name}");
                Console.WriteLine($"🎂 Age            : {vet.Age}");
                Console.WriteLine($"🏠 Address        : {vet.Address}");
                Console.WriteLine($"📞 Phone          : {vet.Phone}");
                Console.WriteLine($"💼 Specialization : {vet.Specialization}");

                if (vet.ConsultationIds != null && vet.ConsultationIds.Count > 0)
                    Console.WriteLine($"📋 Consultations  : {vet.ConsultationIds.Count}");
                else
                    Console.WriteLine($"📋 Consultations  : None");

                Console.WriteLine(new string('-', 40));
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        // ✅ READ ONE VETERINARIAN
        public static async Task ShowAsync()
        {
            string id = Validations.ValidateContent("Enter Veterinarian Firebase ID: ");
            var vet = await _repository.ObtenerPorIdAsync(id);

            if (vet == null)
            {
                Console.WriteLine("❌ Veterinarian not found.");
                return;
            }

            Veterinarian.ShowInformation(vet);
        }

        // ✅ UPDATE VETERINARIAN
        public static async Task UpdateAsync()
        {
            string id = Validations.ValidateContent("Enter Veterinarian Firebase ID to update: ");
            var existing = await _repository.ObtenerPorIdAsync(id);

            if (existing == null)
            {
                Console.WriteLine("❌ Veterinarian not found.");
                return;
            }

            existing.Name = Validations.ValidateContent("Enter new name: ");
            existing.Age = Validations.ValidateNumber("Enter new age: ");
            existing.Address = Validations.ValidateContent("Enter new address: ");
            existing.Phone = Validations.ValidateContent("Enter new phone: ");
            existing.Specialization = Validations.ValidateContent("Enter new specialization: ");

            await _repository.ActualizarAsync(id, existing);
            Console.WriteLine("✅ Veterinarian updated successfully!");
        }

        // ✅ DELETE VETERINARIAN
        public static async Task DeleteAsync()
        {
            string id = Validations.ValidateContent("Enter Veterinarian Firebase ID to delete: ");
            await _repository.EliminarAsync(id);
            Console.WriteLine("🗑️ Veterinarian deleted successfully!");
        }
    }
}
