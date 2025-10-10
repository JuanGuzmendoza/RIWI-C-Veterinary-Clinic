using System.Text;
using System.Text.Json;
using VeterinaryClinic.Models;
using VeterinaryClinic.Interfaces;

namespace VeterinaryClinic.Repositories
{
    public class ConsultationRepository : IRepository<Consultation>
    {
        private readonly string baseUrl = "https://crud1-ab551-default-rtdb.firebaseio.com/consultations";
        private readonly HttpClient client = new HttpClient();

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Converters = { new GuidToStringConverter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
        };

        // ✅ CREAR
        public async Task<string> CrearAsync(Consultation consultation)
        {
            var json = JsonSerializer.Serialize(consultation, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{baseUrl}/{consultation.Id}.json", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(result);

            if (doc.RootElement.TryGetProperty("name", out var nameProp))
            {
                string id = nameProp.GetString();
                Console.WriteLine($"✅ Consultation created with ID: {id}");
                return id;
            }

            Console.WriteLine($"✅ Consultation created with custom ID: {consultation.Id}");
            return consultation.Id.ToString();
        }

        // 📖 OBTENER TODOS
        public async Task<Dictionary<string, Consultation>> ObtenerTodosAsync()
        {
            var response = await client.GetAsync($"{baseUrl}.json");
            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json) || json == "null")
                return new Dictionary<string, Consultation>();

            return JsonSerializer.Deserialize<Dictionary<string, Consultation>>(json, _jsonOptions)
                   ?? new Dictionary<string, Consultation>();
        }

        // 🔍 OBTENER POR ID
        public async Task<Consultation> ObtenerPorIdAsync(string id)
        {
            var response = await client.GetAsync($"{baseUrl}/{id}.json");
            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json) || json == "null")
                return null;

            return JsonSerializer.Deserialize<Consultation>(json, _jsonOptions);
        }

        // ✏️ ACTUALIZAR
        public async Task ActualizarAsync(string id, Consultation consultation)
        {
            var json = JsonSerializer.Serialize(consultation, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{baseUrl}/{id}.json", content);
            response.EnsureSuccessStatusCode();

            Console.WriteLine($"🧾 Consultation {id} updated successfully.");
        }

        // ❌ ELIMINAR
        public async Task EliminarAsync(string id)
        {
            var response = await client.DeleteAsync($"{baseUrl}/{id}.json");
            response.EnsureSuccessStatusCode();

            Console.WriteLine($"🗑️ Consultation {id} deleted successfully.");
        }
    }
}
