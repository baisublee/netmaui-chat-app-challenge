using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

public class VoiceService
{
    private static readonly Lazy<VoiceService> lazyInstance = new Lazy<VoiceService>(() => new VoiceService());
    private static readonly HttpClient httpClient = new HttpClient();

    private const string ApiUrl = "http://localhost:4000/generate_voice";

    // Private constructor to prevent direct instantiation
    private VoiceService()
    {
        // Initialize HttpClient or other necessary settings
    }

    // Public property to provide access to the singleton instance
    public static VoiceService Instance
    {
        get
        {
            return lazyInstance.Value;
        }
    }

    // Method to send text to the voice generation API
    public async Task<string> GenerateVoiceAsync(string text)
    {
        // var escapedText = System.Web.HttpUtility.JavaScriptStringEncode(text);
        // var escapedText = JsonSerializer.Serialize(text);
        // var jsonPayload = new StringContent($"{{\"text\": \"{escapedText}\"}}", Encoding.UTF8, "application/json");

        // HttpResponseMessage response = await httpClient.PostAsync(ApiUrl, jsonPayload);

        var payload = new
        {
            text = text,
        };

        // Serialize the payload to JSON
        var jsonPayload = JsonSerializer.Serialize(payload);
        var jsonPayloadContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync(ApiUrl, jsonPayloadContent);

        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
        else
        {
            throw new Exception($"Error: {response.StatusCode}, {response.ReasonPhrase}");
        }
    }
}

