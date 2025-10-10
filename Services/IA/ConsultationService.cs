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

            Console.WriteLine($"\n[LOG] 🧍‍♂️ CustomerId recibido: {customerId}");
            Console.WriteLine("[LOG] 🔄 Obteniendo veterinarios desde Firebase...");

            // ✅ 1️⃣ Obtener veterinarios directamente del repositorio (Firebase)
            var vets = await _vetRepo.ObtenerTodosAsync();

            Console.WriteLine($"[LOG] ✅ Veterinarios obtenidos: {vets?.Count ?? 0}");

            if (vets == null || vets.Count == 0)
            {
                Console.WriteLine("❌ No hay veterinarios registrados actualmente.");
                Console.ReadKey();
                return;
            }

            // ✅ 2️⃣ Usar el servicio Gemini para seleccionar un veterinario
            Console.WriteLine("[LOG] 🤖 Enviando síntomas al servicio Gemini...");
            var result = await GeminiService.SelectVeterinarianAsync(symptoms, vets);

            if (result == null)
            {
                Console.WriteLine("[ERROR] ❌ GeminiService devolvió null.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\n[LOG] 🎯 Veterinario seleccionado por Gemini:");
            Console.WriteLine($"[LOG]     ➤ SelectedVeterinarianId: {result.SelectedVeterinarianId}");
            Console.WriteLine($"[LOG]     ➤ Reason: {result.Reason}");

            // ✅ 3️⃣ Crear la nueva consulta
            var consultation = new Consultation
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                VeterinarianId = result.SelectedVeterinarianId,
                Description = symptoms,
                Date = DateTime.Now
            };

            Console.WriteLine($"\n[LOG] 🧾 Nueva consulta creada:");
            Console.WriteLine($"[LOG]     ➤ Consultation.Id: {consultation.Id}");
            Console.WriteLine($"[LOG]     ➤ Consultation.VeterinarianId: {consultation.VeterinarianId}");
            Console.WriteLine($"[LOG]     ➤ Consultation.CustomerId: {consultation.CustomerId}");

            // ✅ 4️⃣ Guardar la consulta en Firebase
            Console.WriteLine("[LOG] 💾 Guardando consulta en Firebase...");
            await _consultRepo.CrearAsync(consultation);
            Console.WriteLine("[LOG] ✅ Consulta guardada correctamente.");

            // ✅ 5️⃣ Buscar el veterinario seleccionado
            var selectedVet = vets.Values.FirstOrDefault(v => v.Id == result.SelectedVeterinarianId);

            if (selectedVet == null)
            {
                Console.WriteLine("[ERROR] ❌ No se encontró el veterinario seleccionado en la lista.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\n[LOG] 👨‍⚕️ Veterinario encontrado:");
            Console.WriteLine($"[LOG]     ➤ Vet.Id: {selectedVet.Id}");
            Console.WriteLine($"[LOG]     ➤ Nombre: {selectedVet.Name}");
            Console.WriteLine($"[LOG]     ➤ Consultations antes: {selectedVet.ConsultationIds?.Count ?? 0}");

            // ✅ 6️⃣ Actualizar la lista de consultas
            selectedVet.ConsultationIds ??= new List<Guid>();
            selectedVet.ConsultationIds.Add(consultation.Id);

            Console.WriteLine($"[LOG] ➕ Consulta agregada. Total ahora: {selectedVet.ConsultationIds.Count}");

            // ✅ 7️⃣ Guardar veterinario actualizado en Firebase
            Console.WriteLine($"[LOG] 💾 Actualizando veterinario {selectedVet.Id} en Firebase...");
      await _vetRepo.ActualizarCampoAsync(selectedVet.Id.ToString(), "consultationIds", selectedVet.ConsultationIds);

            Console.WriteLine("[LOG] ✅ Veterinario actualizado correctamente.");

            // ✅ 8️⃣ Sincronizar DataStore
            DataStore.Veterinarians[selectedVet.Id.ToString()] = selectedVet;

            Console.WriteLine("\n🐾 Consulta registrada correctamente.");
            Console.WriteLine("\nPresiona cualquier tecla para volver al menú...");
            Console.ReadKey();
        }
    }
}
