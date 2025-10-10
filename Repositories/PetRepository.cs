using System.Text;
using System.Text.Json;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public class PetRepository : IRepository<Pet>
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string baseUrl = "https://crud1-ab551-default-rtdb.firebaseio.com/pets";

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Converters = { new GuidToStringConverter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
        };

        public async Task<string> CrearAsync(Pet pet)
        {
            var json = JsonSerializer.Serialize(pet, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{baseUrl}/{pet.Id}.json", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);
            if (doc.RootElement.TryGetProperty("name", out var nameProp))
            {
                string id = nameProp.GetString();
                Console.WriteLine($"✅ Mascota creada con ID: {id}");
                return id;
            }   

            return null;
        }

        public async Task<Dictionary<string, Pet>> ObtenerTodosAsync()
        {
            var response = await client.GetAsync($"{baseUrl}.json");
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Dictionary<string, Pet>>(json, _jsonOptions);
        }

        public async Task<Pet> ObtenerPorIdAsync(string id)
        {
            var response = await client.GetAsync($"{baseUrl}/{id}.json");
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Pet>(json, _jsonOptions);
        }

        public async Task ActualizarAsync(string id, Pet pet)
        {
            var json = JsonSerializer.Serialize(pet, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PutAsync($"{baseUrl}/{id}.json", content);
        }

        public async Task EliminarAsync(string id)
        {
            await client.DeleteAsync($"{baseUrl}/{id}.json");
        }
    }
}
