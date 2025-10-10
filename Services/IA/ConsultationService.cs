using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;
using VeterinaryClinic.Data;
using VeterinaryClinic.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

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

            // 1️⃣ Obtener veterinarios del DataStore
            var vets = DataStore.Veterinarians;

            // 2️⃣ Usar el servicio Gemini para seleccionar uno
            var result = await GeminiService.SelectVeterinarianAsync(symptoms, vets);

            Console.WriteLine($"\n✅ Veterinario seleccionado: {result.SelectedVeterinarianId}");
            Console.WriteLine($"📋 Motivo: {result.Reason}");

            // 3️⃣ Crear la nueva consulta
        var consultation = new Consultation
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            VeterinarianId = result.SelectedVeterinarianId,
            Description = symptoms,
            Date = DateTime.Now
        };


            // 4️⃣ Guardar la consulta
            await _consultRepo.CrearAsync(consultation);

            // 5️⃣ Actualizar el veterinario con la nueva consulta
            var selectedVet = vets.First(v => v.Value.Id == result.SelectedVeterinarianId).Value;
            selectedVet.ConsultationIds.Add(consultation.Id);
            await _vetRepo.ActualizarAsync(selectedVet.Id.ToString(), selectedVet);

            Console.WriteLine("\n🐾 Consulta registrada correctamente.");
        }
    }
}
