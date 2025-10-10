using System.Text;
using System.Text.Json;
using VeterinaryClinic.Models;
using  VeterinaryClinic.Interfaces;

namespace VeterinaryClinic.Repositories
{
    public class VeterinarianRepository : IRepository<Veterinarian>
    {
        private readonly string baseUrl = "https://crud1-ab551-default-rtdb.firebaseio.com/Veterinarians";
        private readonly HttpClient client = new HttpClient();

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Converters = { new GuidToStringConverter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
        };

        // CREATE
        public async Task<string> CrearAsync(Veterinarian vet)
        {
            var json = JsonSerializer.Serialize(vet, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{baseUrl}/{vet.Id}.json", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);
            if (doc.RootElement.TryGetProperty("name", out var nameProp))
            {
                string id = nameProp.GetString();
                Console.WriteLine($"✅ Veterinarian created with ID: {id}");
                return id;
            }

            return null;
        }

        // READ ALL
        public async Task<Dictionary<string, Veterinarian>> ObtenerTodosAsync()
        {
            var response = await client.GetAsync($"{baseUrl}.json");
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Dictionary<string, Veterinarian>>(json, _jsonOptions);
        }

        // READ BY ID
        public async Task<Veterinarian> ObtenerPorIdAsync(string id)
        {
            var response = await client.GetAsync($"{baseUrl}/{id}.json");
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Veterinarian>(json, _jsonOptions);
        }

        // UPDATE
        public async Task ActualizarAsync(string id, Veterinarian vet)
        {
            var json = JsonSerializer.Serialize(vet, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PutAsync($"{baseUrl}/{id}.json", content);
        }

        // DELETE
        public async Task EliminarAsync(string id)
        {
            await client.DeleteAsync($"{baseUrl}/{id}.json");
        }
    }
}
