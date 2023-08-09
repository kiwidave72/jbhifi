namespace JbHiFi.OpenWeather.Client;

public interface IOpenWeatherResponse
{
    
    bool IsSuccess { get; }
    string ErrorMessage { get; }

}