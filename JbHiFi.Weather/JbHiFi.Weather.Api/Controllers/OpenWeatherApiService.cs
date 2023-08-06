using Microsoft.Extensions.Options;

namespace JbHiFi.Weather.Api.Controllers;

public class OpenWeatherApiService : IOpenWeatherApiService
{
    private readonly IOptions<OpenWeatherAPISettings> _options;

    private readonly HttpClient _httpClient;

    public OpenWeatherApiService(IOptions<OpenWeatherAPISettings> options, HttpClient httpClient)
    {
        _options = options;
        _httpClient = httpClient;
    }

    public async Task<string> GetWeather()
    {
        
        var url = _options.Value.BaseUrl + "/weather?q=London,uk&appid=" + _options.Value.AppId;
        
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Request failed.");
        }

        var responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;
    }



}