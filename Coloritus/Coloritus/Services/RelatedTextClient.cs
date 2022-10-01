using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Coloritus.Services.Abstract;

namespace Coloritus.Services;

public class RelatedTextClient : IRelatedTextClient
{
    private readonly HttpClient _httpClient;

    public RelatedTextClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetRelatedText(string input)
    {
        var result = await _httpClient.GetAsync("/words?sl=" + HttpUtility.UrlEncode(input));
        var body = await result.Content.ReadAsStringAsync();
        var json = System.Text.Json.JsonDocument.Parse(body);

        var rand = new Random();

        var jsonElement = json.RootElement
            .EnumerateArray()
            .ElementAt(rand.Next(json.RootElement.GetArrayLength()))
            .GetProperty("word")
            .GetString();

        return jsonElement;
    }
}