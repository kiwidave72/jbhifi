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
    [InlineData("/Api/Forecast", "ClientAppId1")]
    public async Task IntegratesWithAuthentication(string url,string appId)
    {
        var client = _factory.CreateClient();
        HttpResponseMessage response = null;

        var exception = await Record.ExceptionAsync(async () => response = await client.GetAsync($"{url}?appId={appId}&city=london&country=uk"));
        Assert.True(exception==null);
        Assert.True(response.IsSuccessStatusCode);

    }
}