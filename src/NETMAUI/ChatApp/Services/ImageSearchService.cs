using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

public class ImageSearchService
{
    private static readonly Lazy<ImageSearchService> lazyInstance =
        new Lazy<ImageSearchService>(() => new ImageSearchService());

    private readonly HttpClient httpClient;
    private const string BaseUrl = "http://localhost:3000"; // Change this if your service is hosted elsewhere

    private ImageSearchService()
    {
        httpClient = new HttpClient();
    }

    public static ImageSearchService Instance
    {
        get { return lazyInstance.Value; }
    }

    public async Task<string> SearchImageAsync(string query)
    {
        var payload = new { query = query };
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync($"{BaseUrl}/search", content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<SearchResponse>(jsonResponse);
            return responseData.ImageUrl;
        }
        else
        {
            throw new Exception($"Error: {response.StatusCode}, {response.ReasonPhrase}");
        }
    }

    private class SearchResponse
    {
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }
    }
}
