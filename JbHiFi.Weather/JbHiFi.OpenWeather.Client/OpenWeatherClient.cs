using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using JbHiFi.OpenWeather.Client.Models;
using JbHiFi.Weather.Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JbHiFi.OpenWeather.Client;

public class OpenWeatherClient : IOpenWeatherClient
{
    private readonly ILogger<OpenWeatherClient> _logger;

    private readonly IOptions<OpenWeatherAPISettings> _options;

    private readonly HttpClient _httpClient;

    public OpenWeatherClient(IOptions<OpenWeatherAPISettings> options, HttpClient httpClient, ILogger<OpenWeatherClient> logger)
    {
        _options = options;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<OpenWeatherResponse<OpenWeatherModel>> GetWeather(string city,string country)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var url = _options.Value.BaseUrl + $"/weather?q={city},{country}&appid=" + _options.Value.AppId;
        
        var response = await _httpClient.GetAsync(url);

        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                return new OpenWeatherResponse<OpenWeatherModel>(null, false, "OpenWeather Rate Limit exceeded.");
            }

            var errorModel = JsonSerializer.Deserialize<OpenWeatherErrorModel>(responseBody, options);
            return new OpenWeatherResponse<OpenWeatherModel>(null, false, $"OpenWeather Request failed with {errorModel.Title}");

        }

        var model = JsonSerializer.Deserialize<OpenWeatherModel>(responseBody, options);

        return new OpenWeatherResponse<OpenWeatherModel>(model);
    }


  
}