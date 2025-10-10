using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using VeterinaryClinic.Models;

public static class GeminiService
{
    private static readonly string _apiKey = "AIzaSyCNZZPfPm3AgVYQyb_CNFWGmzuCO532DM8";
    private static readonly HttpClient _client = new();
    private static readonly string _baseUrl =
        "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

    public static async Task<SelectedVetResult> SelectVeterinarianAsync(string symptoms, Dictionary<string, Veterinarian> vets)
    {
        var vetList = string.Join(", ", vets.Values.Select(v => $"{v.Id}:{v.Name}-{v.Specialization}"));

        var prompt = $@"
Tengo los siguientes veterinarios: {vetList}.
Basado en los síntomas del paciente: ""{symptoms}"",
elige el veterinario más adecuado y responde SOLO en formato JSON EXACTAMENTE así:
{{
  ""selectedVeterinarianId"": ""id_del_veterinario"",
  ""reason"": ""explicación breve de por qué lo elegiste""
}}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var jsonBody = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"{_baseUrl}?key={_apiKey}", content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error de Gemini API: {response.StatusCode} - {error}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(jsonResponse);
        var text = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        text = text?
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

        SelectedVetResult? result = null;

        try
        {
            result = JsonSerializer.Deserialize<SelectedVetResult>(text, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            // En caso de error, devolvemos un resultado vacío
        }

        return result ?? new SelectedVetResult
        {
            SelectedVeterinarianId = Guid.Empty,
            Reason = "No se pudo determinar un veterinario."
        };
    }
}
