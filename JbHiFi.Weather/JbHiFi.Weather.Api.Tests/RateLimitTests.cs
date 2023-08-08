using Microsoft.Extensions.Options;
using Xunit.Abstractions;

namespace JbHiFi.Weather.Api.Tests
{
    public class RateLimitTests
    {
        private readonly IOptions<RateLimiterSettings> _testSettings;
        private readonly XunitLogger<RateLimiter> _logger;
        
        public RateLimitTests(ITestOutputHelper output)
        {
            _logger = new XunitLogger<RateLimiter>(output);
           
            _testSettings = Options.Create(new RateLimiterSettings()
            {
                Limit = 5,
                Time = 60 
            });
        }

        [Fact]
        public void RatelimitIntegration()
        {

            var window = new RateLimitTimeProxy(DateTime.Parse("2023/01/01 00:0:00"));

            var rateLimit = new RateLimiter(_logger, _testSettings, window ,new SlidingWindowValidator(_logger));

            rateLimit.RecordRequestForAppId("TestApp1");

            window.SetDateTime(window.GetDateTime().AddMinutes(20));
            rateLimit.RecordRequestForAppId("TestApp1");

            window.SetDateTime(window.GetDateTime().AddMinutes(10));
            rateLimit.RecordRequestForAppId("TestApp1");

            window.SetDateTime(window.GetDateTime().AddMinutes(10));
            rateLimit.RecordRequestForAppId("TestApp1");

            window.SetDateTime(window.GetDateTime().AddMinutes(10));
            rateLimit.RecordRequestForAppId("TestApp1");



            // sixth call
            window.SetDateTime(window.GetDateTime().AddMinutes(5));

            var exception = Record.Exception(() => rateLimit.RecordRequestForAppId("TestApp1"));

            Assert.True(exception.Message.Contains("rate limit exceeded"));


        }
    }
}
