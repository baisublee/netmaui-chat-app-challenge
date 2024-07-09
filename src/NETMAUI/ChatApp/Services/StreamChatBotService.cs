using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

public class StreamChatBotService
{
    private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };
    private static StreamChatBotService _instance;

    // Private constructor to prevent direct instantiation
    private StreamChatBotService() { }

    // Singleton instance
    public static StreamChatBotService Instance
    {
        get
        {
            if (_instance == null)
                _instance = new StreamChatBotService();

            return _instance;
        }
    }

    public async Task GenerateResponseAsync(string userInput, double temp = 0.0, int maxTokens = 1000, Action<string> onChunkReceived = null)
    {
        var payload = new
        {
            prompt = userInput,
            temp = temp,
            max_tokens = maxTokens
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("generate_response", content);
        response.EnsureSuccessStatusCode();

        var responseStream = await response.Content.ReadAsStreamAsync();

        using (var reader = new StreamReader(responseStream))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var chunk = JsonSerializer.Deserialize<StreamResponseChunk>(line);
                onChunkReceived?.Invoke(chunk.Chunk);
            }
        }
    }

    private class StreamResponseChunk
    {
        [JsonPropertyName("chunk")]
        public string Chunk { get; set; }
    }
}
