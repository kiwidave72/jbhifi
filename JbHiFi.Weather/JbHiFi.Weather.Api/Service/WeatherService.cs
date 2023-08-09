using JbHiFi.OpenWeather.Client;
using JbHiFi.Weather.Api.Controllers;
using JbHiFi.Weather.Api.Models;

namespace JbHiFi.Weather.Api.Service;

public class WeatherService : IWeatherService
{
    private readonly ILogger<WeatherService> _logger;
    private readonly IOpenWeatherClient _openWeatherApi;

    public WeatherService(ILogger<WeatherService> logger, IOpenWeatherClient openWeatherApi)
    {
        _logger = logger;
        _openWeatherApi = openWeatherApi;
    }


    public async Task<WeatherModel> GetWeather(string city, string country)
    {

        _logger.LogInformation("try fetching GetWeather");
        var openWeather = await _openWeatherApi.GetWeather(city, country);

        WeatherModel weather = new WeatherModel();

        weather.Description = openWeather.Weather.First().Description;

        return weather;
    }


}