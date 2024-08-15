using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Diagnostics;


public class EdgeAIService
{
    private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:5000/") };

    private static EdgeAIService _instance;
    private const string UaString = "Your-UA-String-Here"; // Replace with your actual UA string

    private EdgeAIService() 
    {
        client.DefaultRequestHeaders.Add("x-ua-string", UaString);
    }

    public static EdgeAIService Instance => _instance ??= new EdgeAIService();

    public async Task GetCharactersAsync(Action<List<Character>> onCharactersReceived)
    {
        try
        {
            var response = await client.GetStringAsync("characters");
            var characters = JsonSerializer.Deserialize<List<Character>>(response);
            onCharactersReceived?.Invoke(characters);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetCharactersAsync: {ex.Message}");
        }
    }

    public async Task CreateCharacterAsync(CharacterCreationRequest characterRequest, Action<Character> onCharacterCreated)
    {
        try
        {
            var jsonPayload = JsonSerializer.Serialize(characterRequest);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("characters", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var createdCharacter = JsonSerializer.Deserialize<Character>(responseBody);
            onCharacterCreated?.Invoke(createdCharacter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateCharacterAsync: {ex.Message}");
        }
    }

    public async Task StartChatAsync(string characterId, string message, Action<string> onPartialResponse)
    {
        try
        {
            var payload = new { message };
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"characters/{characterId}/chat")
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
                    Debug.WriteLine(line);
                    var chatResponse = JsonSerializer.Deserialize<ChatMessageResponse>(line);
                    Debug.WriteLine(chatResponse);
                    if (chatResponse != null && chatResponse.Data != null)
                    {
                        onPartialResponse(chatResponse.Data.Response);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in StartChatAsync: {ex.Message}");
        }
    }

public class Character
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("character_name")]
    public string CharacterName { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("first_message")]
    public string FirstMessage { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("is_user_defined_character")]
    public bool IsUserDefinedCharacter { get; set; }
}

public class CharacterCreationRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("first_message")]
    public string FirstMessage { get; set; }
}

public class ChatMessageResponse
{
    [JsonPropertyName("data")]
    public ChatMessage Data { get; set; }
}

public class ChatMessage
{
    [JsonPropertyName("character_id")]
    public string CharacterId { get; set; }

    [JsonPropertyName("response")]
    public string Response { get; set; }

    [JsonPropertyName("createdAt")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("done")]
    public bool Done { get; set; }
}
}