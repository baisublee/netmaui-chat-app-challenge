using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        var jsonPayload = new StringContent($"{{\"text\": \"{text}\"}}", Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync(ApiUrl, jsonPayload);

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
