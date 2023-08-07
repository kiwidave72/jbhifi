using JbHiFi.Weather.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using WeatherModel = JbHiFi.Weather.Api.Models.WeatherModel;

public class SmokeTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public SmokeTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/Api/Forecast")]
    public async Task OpenWeatherAPIKeyReturnsOK(string url)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);
        
        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);


        var model = JsonSerializer.Deserialize<WeatherModel>(responseBody, options);

        Assert.True(model.Description.Contains("clear sky"));

    }
}