using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class OllamaChatBotService
{
    private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:11434/") };
    private static OllamaChatBotService _instance;

    private readonly List<ChatMessage> conversationHistory;
    private bool useStreaming;

    // Private constructor to prevent direct instantiation
    private OllamaChatBotService()
    {
        conversationHistory = new List<ChatMessage>();
        useStreaming = true;
    }

    // Singleton instance
    public static OllamaChatBotService Instance
    {
        get
        {
            if (_instance == null)
                _instance = new OllamaChatBotService();

            return _instance;
        }
    }

    public void EnableStreaming(bool enable)
    {
        useStreaming = enable;
    }

    public async Task SendMessageAsync(string userMessage, Action<string> onPartialResponse)
    {
        conversationHistory.Add(new ChatMessage { Role = "user", Content = userMessage });

        var promptBuilder = new StringBuilder();
        foreach (var message in conversationHistory)
        {
            promptBuilder.AppendLine($"{message.Role}: {message.Content}");
        }

        var payload = new
        {
            model = "llama3",
            prompt = promptBuilder.ToString(),
            stream = useStreaming
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/generate")
            {
                Content = content
            };

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();

            using (var reader = new StreamReader(responseStream))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var chunk = JsonSerializer.Deserialize<StreamedChatResponse>(line);
                    if (!string.IsNullOrEmpty(chunk.Response))
                    {
                        onPartialResponse(chunk.Response);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SendMessageAsync: {ex.Message}");
        }
    }

    private class ChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    private class StreamedChatResponse
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("response")]
        public string Response { get; set; }

        [JsonPropertyName("done")]
        public bool Done { get; set; }
    }
}
