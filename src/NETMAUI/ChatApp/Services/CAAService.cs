using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Diagnostics;

public class CAAService
{
    private static readonly Lazy<CAAService> _instance = new Lazy<CAAService>(() => new CAAService());

    private readonly HttpClient _httpClient;

    // Constant for the User-Agent string
    private const string UserAgentString = "EdgeAI/1.0 (Windows NT 10.0; Win64; x64)";

    public static CAAService Instance => _instance.Value;

    private CAAService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8000") };
    }

    // Method to retrieve existing characters
    public async Task<List<Character>> GetCharactersAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/characters");
        request.Headers.Add("x-ua-string", UserAgentString);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // Get the raw JSON string response
        var responseBody = await response.Content.ReadAsStringAsync();
        Debug.WriteLine("Main Page GetCharactersAsync: " + responseBody);
        var characterResponse = JsonSerializer.Deserialize<CharacterListResponse>(responseBody);
        Debug.WriteLine("Main Page GetCharactersAsync: " + characterResponse.Data.Count);

        // var responseBody = await response.Content.ReadAsStringAsync();
        // var characters = JsonSerializer.Deserialize<List<Character>>(responseBody);

        return characterResponse.Data;
    }

    // Method to create a new character
    public async Task<Character> CreateCharacterAsync(object characterRequest)
    {
        var jsonPayload = JsonSerializer.Serialize(characterRequest);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/characters", content);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var createdCharacterResponse = JsonSerializer.Deserialize<CharacterResponse>(responseBody);

        return createdCharacterResponse.Data;
    }

    // Method to start a chat with a selected character, handling streaming responses
    public async Task StartChatAsync(string characterId, string message, Action<string, bool> onPartialResponse)
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

            // Set user agent header using the constant
            request.Headers.Add("x-ua-string", UserAgentString);

            // Send request and get response
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();

            using (var reader = new System.IO.StreamReader(responseStream))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    // Deserialize each line into ChatMessageResponse
                    var chatResponse = JsonSerializer.Deserialize<ChatMessageResponse>(line);
                    if (chatResponse != null && chatResponse.Data != null)
                    {
                        // Invoke the callback with the partial response
                        onPartialResponse(chatResponse.Data.Response, chatResponse.Data.Done);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in StartChatAsync: {ex.Message}");
        }
    }

    // Method to generate an image
    public async Task<ImageGenerationResponse> GenerateImageAsync(object imageRequest)
    {
        var jsonPayload = JsonSerializer.Serialize(imageRequest);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/images/generate", content);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var imageResponse = JsonSerializer.Deserialize<ImageGenerationResponse>(responseBody);

        return imageResponse;
    }
}

// Data model for character description
public class CharacterDescription
{
    [JsonPropertyName("gender")]
    public string Gender { get; set; }

    [JsonPropertyName("pronouns")]
    public string Pronouns { get; set; }

    [JsonPropertyName("stage_of_life")]
    public string StageOfLife { get; set; }

    [JsonPropertyName("personality")]
    public string Personality { get; set; }

    [JsonPropertyName("details")]
    public string Details { get; set; }

    [JsonPropertyName("greeting_message")]
    public string GreetingMessage { get; set; }
}

// Data model for character
public class Character
{
    public string AvatarImage { get; set; } = "rachel_image.png";

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("character_name")]
    public string CharacterName { get; set; }

    [JsonPropertyName("description")]
    public CharacterDescription Description { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; } // Milliseconds since Epoch

    [JsonPropertyName("is_user_defined_character")]
    public bool IsUserDefinedCharacter { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is Character otherUser)
        {
            return this.Id == otherUser.Id;
        }
        return false;
    }

    // Override GetHashCode to use the Id for generating hash code
    public override int GetHashCode()
    {
        return Id?.GetHashCode() ?? 0;
    }

}

// Data model for chat response
public class ChatMessageResponse
{
    [JsonPropertyName("data")]
    public ChatResponse Data { get; set; }
}

public class ChatResponse
{
    [JsonPropertyName("character_id")]
    public string CharacterId { get; set; }

    [JsonPropertyName("response")]
    public string Response { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; } // Milliseconds since Epoch

    [JsonPropertyName("done")]
    public bool Done { get; set; }
}

// Data model for image generation response
public class ImageGenerationResponse
{
    [JsonPropertyName("type")]
    public string Type { get; set; } // MIME type

    [JsonPropertyName("image_data")]
    public string ImageData { get; set; } // Base64 encoded image data
}

public class CharacterListResponse
{
    [JsonPropertyName("data")]
    public List<Character> Data { get; set; }
}

public class CharacterResponse
{
    [JsonPropertyName("data")]
    public Character Data { get; set; }
}