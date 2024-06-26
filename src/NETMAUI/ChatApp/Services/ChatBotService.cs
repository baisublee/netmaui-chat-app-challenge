using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

public class ChatBotService
{
    private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };
    private static ChatBotService _instance;

    // Private constructor to prevent direct instantiation
    private ChatBotService() { }

    // Singleton instance
    public static ChatBotService Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ChatBotService();

            return _instance;
        }
    }

    public async Task<string> GenerateResponseAsync(string userInput, double temp = 0.0, int maxTokens = 1000)
    {
        // Create the request payload
        var payload = new
        {
            prompt = userInput,
            temp = temp,
            max_tokens = maxTokens
        };

        // Serialize the payload to JSON
        var jsonPayload = JsonSerializer.Serialize(payload);

        // Create the content for the POST request
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Make the POST request to the generate_response endpoint
        var response = await client.PostAsync("generate_response", content);

        // Ensure the request was successful
        response.EnsureSuccessStatusCode();

        // Read and deserialize the response content
        var responseString = await response.Content.ReadAsStringAsync();
        var responseJson = JsonSerializer.Deserialize<GenerateResponseResult>(responseString);

        // Return the response content from the model
        return responseJson.Response;
    }

 private class GenerateResponseResult
    {
        [JsonPropertyName("response")]
        public string Response { get; set; }
        
        [JsonPropertyName("num_tokens_generated")]
        public int NumTokensGenerated { get; set; }
        
        [JsonPropertyName("generation_time")]
        public double GenerationTime { get; set; }
        
        [JsonPropertyName("generation_tps")]
        public double GenerationTps { get; set; }
        
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }}
