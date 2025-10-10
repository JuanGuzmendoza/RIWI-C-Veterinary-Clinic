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

    if (!response.IsSuccessStatusCode)
        return new Dictionary<string, Veterinarian>();

    var json = await response.Content.ReadAsStringAsync();

    var result = JsonSerializer.Deserialize<Dictionary<string, Veterinarian>>(json, _jsonOptions)
                 ?? new Dictionary<string, Veterinarian>();

    // 🧩 Asignar el ID (clave del diccionario) a la propiedad Id del objeto
    foreach (var kvp in result)
    {
        if (Guid.TryParse(kvp.Key, out var guid))
            kvp.Value.Id = guid;
    }

    return result;
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

    var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{baseUrl}/{id}.json")
    {
        Content = content
    };

    var response = await client.SendAsync(request);
    response.EnsureSuccessStatusCode();
}

public async Task ActualizarCampoAsync(string id, string field, object value)
{
    try
    {
        // 🔹 Si el valor es una lista de Guid, convertirla a lista de string
        if (value is List<Guid> guidList)
            value = guidList.Select(g => g.ToString()).ToList();

        var json = JsonSerializer.Serialize(value, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        string url = $"{baseUrl}/{id}/{field}.json";

        // 🔍 Verificar si el valor es una lista o array
        bool isArray = value is System.Collections.IEnumerable && value is not string;

        if (isArray)
        {
            // 🧩 Firebase no acepta arrays en PATCH, usar PUT directamente
            Console.WriteLine($"[DEBUG] 🔧 PUT -> {url}");
            Console.WriteLine($"[DEBUG] 📦 JSON enviado (array): {json}");
            var response = await client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
        else
        {
            // ✅ PATCH para valores normales
            Console.WriteLine($"[DEBUG] 🔧 PATCH -> {url}");
            Console.WriteLine($"[DEBUG] 📦 JSON enviado: {json}");
            var response = await client.PatchAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        Console.WriteLine("[LOG] ✅ Campo actualizado correctamente en Firebase.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] ❌ Error al actualizar campo en Firebase: {ex.Message}");
        throw;
    }
}

        // DELETE
        public async Task EliminarAsync(string id)
        {
            await client.DeleteAsync($"{baseUrl}/{id}.json");
        }
    }
}
