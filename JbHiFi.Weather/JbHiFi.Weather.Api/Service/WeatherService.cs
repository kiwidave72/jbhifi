using JbHiFi.OpenWeather.Client;
using JbHiFi.Weather.Api.Response;
using WeatherModel = JbHiFi.Weather.Api.Models.WeatherModel;

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


    public async Task<WeatherResponse<WeatherModel>> GetWeather(string city, string country)
    {

        _logger.LogInformation("try fetching GetWeather");
        
        var response = await _openWeatherApi.GetWeather(city, country);

        if (!response.IsSuccess)
        {
            throw new Exception("Request Errored");
        }
        return new WeatherResponse<WeatherModel>(new WeatherModel(){Description = response.Data.Weather.First().Description });
    }


}