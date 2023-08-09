namespace JbHiFi.Weather.Api.Response;

public class WeatherResponse<T> : IWeatherResponse
{
    public T Data { get; }

    public bool IsSuccess { get; }

    public String ErrorMessage { get; }

    public WeatherResponse(T data, bool isSuccess, string errorMessages)
    {
        Data = data;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessages;
    }

    public WeatherResponse(T data) : this(data, true, string.Empty)
    {
    }



}