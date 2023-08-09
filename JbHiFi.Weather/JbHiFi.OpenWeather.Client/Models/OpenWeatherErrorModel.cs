namespace JbHiFi.OpenWeather.Client.Models;

public class OpenWeatherErrorModel
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string TraceId { get; set; }
    public Errors Errors { get; set; }
}