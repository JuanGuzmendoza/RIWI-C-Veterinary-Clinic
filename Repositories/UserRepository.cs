using System.Text;
using System.Text.Json;
using VeterinaryClinic.Models;
using VeterinaryClinic.Interfaces;
namespace VeterinaryClinic.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly string baseUrl = "https://crud1-ab551-default-rtdb.firebaseio.com/Users";
        private readonly HttpClient client = new HttpClient();

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        // 🔹 CREATE
        public async Task<string> CrearAsync(User user)
        {
            var json = JsonSerializer.Serialize(user, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{baseUrl}/{user.Id}.json", content);
            response.EnsureSuccessStatusCode();

            return user.Id.ToString();
        }

        // 🔹 READ ALL
        public async Task<Dictionary<string, User>> ObtenerTodosAsync()
        {
            var response = await client.GetAsync($"{baseUrl}.json");
            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json) || json == "null")
                return new Dictionary<string, User>();

            return JsonSerializer.Deserialize<Dictionary<string, User>>(json, _jsonOptions)
                   ?? new Dictionary<string, User>();
        }

        // 🔹 READ BY ID
        public async Task<User?> ObtenerPorIdAsync(string id)
        {
            var response = await client.GetAsync($"{baseUrl}/{id}.json");
            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json) || json == "null")
                return null;

            return JsonSerializer.Deserialize<User>(json, _jsonOptions);
        }

        // 🔹 UPDATE
        public async Task ActualizarAsync(string id, User user)
        {
            var json = JsonSerializer.Serialize(user, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PutAsync($"{baseUrl}/{id}.json", content);
        }

        // 🔹 DELETE
        public async Task EliminarAsync(string id)
        {
            await client.DeleteAsync($"{baseUrl}/{id}.json");
        }
    }
}
