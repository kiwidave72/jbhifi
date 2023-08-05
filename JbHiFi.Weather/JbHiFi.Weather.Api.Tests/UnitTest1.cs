using JbHiFi.Weather.Api;
using Microsoft.AspNetCore.Mvc.Testing;

public class BasicTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/Forecast")]
    public async Task OpenWeatherAPIKeyReturnsOK(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);
        
        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);

        Assert.True(responseBody.Contains("\"name\":\"London\""));

    }
}