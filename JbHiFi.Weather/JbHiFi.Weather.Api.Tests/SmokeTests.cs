using JbHiFi.Weather.Api;
using Microsoft.AspNetCore.Mvc.Testing;

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
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);
        
        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
         
        Assert.True(responseBody.Contains("\"name\":\"London\""));

    }
}