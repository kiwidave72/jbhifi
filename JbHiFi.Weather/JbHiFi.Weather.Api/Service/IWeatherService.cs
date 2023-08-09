using JbHiFi.Weather.Api.Models;
using JbHiFi.Weather.Api.Response;

namespace JbHiFi.Weather.Api.Service;

public interface IWeatherService
{
    Task<WeatherResponse<WeatherModel>> GetWeather(string city, string country);
}