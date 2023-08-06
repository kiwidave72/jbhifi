namespace JbHiFi.Weather.Api.Controllers;

public interface IOpenWeatherApiService
{
    Task<string> GetWeather();
}