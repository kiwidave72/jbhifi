using JbHiFi.OpenWeather.Client.Models;

namespace JbHiFi.OpenWeather.Client;

public interface IOpenWeatherClient
{
    Task<OpenWeatherResponse<OpenWeatherModel>> GetWeather(string city, string country);
}