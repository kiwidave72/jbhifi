using JbHiFi.Tests.Common;
using JbHiFi.Weather.Api;
using JbHiFi.Weather.Api.RateLimit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
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
    public async Task IntegratesErrorsWithOutAuthentication(string url)
    {
        var client = _factory.CreateClient();
        HttpResponseMessage response = null;

        var exception = await Record.ExceptionAsync(async () => response = await client.GetAsync($"{url}"));
        Assert.True(exception == null);
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(response.StatusCode ,HttpStatusCode.Unauthorized);

    }

    [Theory]
    [InlineData("/Api/Forecast", "ClientAppId1")]
    public async Task IntegratesWithAuthentication(string url, string appId)
    {
        var client = _factory.CreateClient();
        HttpResponseMessage response = null;

        var exception = await Record.ExceptionAsync(async () => response = await client.GetAsync($"{url}?appId={appId}&city=london&country=uk"));
        Assert.True(exception == null);
        Assert.True(response.IsSuccessStatusCode);

    }

    [Theory]
    [InlineData("/Api/Forecast", "ClientAppId1")]
    public async Task  OpenWeatherCanReturnValidationErrors(string url, string appId)
    {
        var client = _factory.CreateClient();
        HttpResponseMessage response = null;

        var exception = await Record.ExceptionAsync(async () => response = await client.GetAsync($"{url}?appId={appId}&city=&country="));
        var responseBody = await response.Content.ReadAsStringAsync();
        
        Assert.True(exception == null);
        Assert.False(response.IsSuccessStatusCode);
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);

        Assert.True(responseBody.Contains("One or more validation errors occurred"));

    }
}