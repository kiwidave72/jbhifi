namespace JbHiFi.OpenWeather.Client.Models;
public class OpenWeatherModel
{
    public List<WeatherModel> Weather { get; set; }

}

public class WeatherModel
{

    public string Description { get; set; }
}