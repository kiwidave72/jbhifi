using JbHiFi.OpenWeather.Client.Models;

namespace JbHiFi.OpenWeather.Client;

public interface IOpenWeatherClient
{
    Task<OpenWeatherModel> GetWeather();
}