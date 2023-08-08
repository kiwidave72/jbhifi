using JbHiFi.Weather.Api;
using JbHiFi.Weather.Api.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using Xunit.Abstractions;

public class SmokeTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;


    private readonly XunitLogger<RateLimiter> _logger;
 

    public SmokeTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _logger = new XunitLogger<RateLimiter>(output);
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
        HttpResponseMessage response = null;

        var exception = await Record.ExceptionAsync(async () => response = await client.GetAsync(url));
        Assert.Null(exception);
        
    }


    [Theory]
    [InlineData(true,"/Api/Forecast")]
    public async Task IntegratesWithRateLimit(bool passRateLimit, string url)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var client = _factory.CreateClient();
        HttpResponseMessage response = null;

        var exception = await Record.ExceptionAsync(async () => response = await client.GetAsync(url));
        Assert.True(exception==null && passRateLimit);
    }
}