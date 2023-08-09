namespace JbHiFi.OpenWeather.Client;

    public class OpenWeatherResponse<T> : IOpenWeatherResponse
    {
        public T Data { get; }

        public bool IsSuccess { get;  }

        public String ErrorMessage { get; }

        public OpenWeatherResponse(T data, bool isSuccess, string errorMessages)
        {
            Data = data;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessages;
        }

        public OpenWeatherResponse(T data) : this(data,true,string.Empty)
        {
        }

    }