using JbHiFi.Weather.Api.Models;

namespace JbHiFi.Weather.Api.Service;

public interface IWeatherService
{
    Task<WeatherModel> GetWeather(string city, string country);
}