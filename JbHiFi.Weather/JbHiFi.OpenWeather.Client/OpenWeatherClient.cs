using System.Text.Json;
using JbHiFi.OpenWeather.Client.Models;
using JbHiFi.Weather.Api;
using Microsoft.Extensions.Options;

namespace JbHiFi.OpenWeather.Client;

public class OpenWeatherClient : IOpenWeatherClient
{
    private readonly IOptions<OpenWeatherAPISettings> _options;

    private readonly HttpClient _httpClient;

    public OpenWeatherClient(IOptions<OpenWeatherAPISettings> options, HttpClient httpClient)
    {
        _options = options;
        _httpClient = httpClient;
    }

    public async Task<OpenWeatherModel> GetWeather(string city,string country)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var url = _options.Value.BaseUrl + $"/weather?q={city},{country}&appid=" + _options.Value.AppId;
        
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Request failed.");
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        
        Console.WriteLine(responseBody);

        var model =  JsonSerializer.Deserialize<OpenWeatherModel>(responseBody, options);


        Console.WriteLine(responseBody);

        return model;
    }



}