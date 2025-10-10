using System.Text;
using System.Text.Json;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public class CustomerRepository : IRepository<Customer>   
    {
        private readonly string baseUrl = "https://crud1-ab551-default-rtdb.firebaseio.com/Customers";
        private readonly HttpClient client = new HttpClient();

private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
{
    Converters = { new GuidToStringConverter() },

    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = false,
};

public async Task<string> CrearAsync(Customer Customer)
{
    var json = JsonSerializer.Serialize(Customer, _jsonOptions);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await client.PutAsync($"{baseUrl}/{Customer.Id}.json", content);
    response.EnsureSuccessStatusCode(); // Verifica que la petición sea correcta

    var result = await response.Content.ReadAsStringAsync();

    using var doc = JsonDocument.Parse(result);
    if (doc.RootElement.TryGetProperty("name", out var nameProp))
    {
        string id = nameProp.GetString();
        Console.WriteLine($"✅ Paciente creado con ID: {id}");
        return id; // Devuelve el ID generado
    }

    return null;
}

        // READ ALL
        public async Task<Dictionary<string, Customer>> ObtenerTodosAsync()
        {
            var response = await client.GetAsync($"{baseUrl}.json");
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Dictionary<string, Customer>>(json, _jsonOptions);
        }

        // READ BY ID
        public async Task<Customer> ObtenerPorIdAsync(string id)
        {
            var response = await client.GetAsync($"{baseUrl}/{id}.json");
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Customer>(json, _jsonOptions);
        }

        // UPDATE
        public async Task ActualizarAsync(string id, Customer Customer)
        {
            var json = JsonSerializer.Serialize(Customer, _jsonOptions);
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
