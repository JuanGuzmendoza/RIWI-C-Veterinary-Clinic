using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;
using VeterinaryClinic.Data;

namespace VeterinaryClinic.Services
{
    public static class ConsultationService
    {
        private static readonly ConsultationRepository _consultRepo = new();
        private static readonly VeterinarianRepository _vetRepo = new();

        public static async Task CreateConsultationAsync(Guid customerId)
        {
            Console.WriteLine("💬 Describe el problema de tu mascota:");
            string symptoms = Console.ReadLine() ?? "";

            var vets = await _vetRepo.ObtenerTodosAsync();

            if (vets == null || vets.Count == 0)
            {
                Console.WriteLine("❌ No hay veterinarios registrados actualmente.");
                Console.ReadKey();
                return;
            }

            var result = await GeminiService.SelectVeterinarianAsync(symptoms, vets);

            if (result == null)
            {
                Console.WriteLine("❌ No se pudo asignar un veterinario en este momento.");
                Console.ReadKey();
                return;
            }

            var consultation = new Consultation
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                VeterinarianId = result.SelectedVeterinarianId,
                Description = symptoms,
                Date = DateTime.Now
            };

            await _consultRepo.CrearAsync(consultation);

            var selectedVet = vets.Values.FirstOrDefault(v => v.Id == result.SelectedVeterinarianId);

            if (selectedVet == null)
            {
                Console.WriteLine("❌ No se encontró el veterinario asignado.");
                Console.ReadKey();
                return;
            }

            selectedVet.ConsultationIds ??= new List<Guid>();
            selectedVet.ConsultationIds.Add(consultation.Id);

            await _vetRepo.ActualizarCampoAsync(
                selectedVet.Id.ToString(), 
                "consultationIds", 
                selectedVet.ConsultationIds
            );

            DataStore.Veterinarians[selectedVet.Id.ToString()] = selectedVet;

            Console.WriteLine("\n🐾 Consulta registrada correctamente.");
            Console.WriteLine("\nPresiona cualquier tecla para volver al menú...");
            Console.ReadKey();
        }
    }
}
