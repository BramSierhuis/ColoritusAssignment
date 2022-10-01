using System.Net.Http;
using System.Threading.Tasks;
using Coloritus.Services.Abstract;

namespace Coloritus.Services;

public class ColorApiClient : IColorApiClient
{
    private readonly HttpClient _httpClient;

    public ColorApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetColorName(string color)
    {
        var result = await _httpClient.GetAsync("/id?hex=" + color);
        var body = await result.Content.ReadAsStringAsync();
        var json = System.Text.Json.JsonDocument.Parse(body);

        return json.RootElement.GetProperty("name").GetProperty("value").GetString();
    }
}