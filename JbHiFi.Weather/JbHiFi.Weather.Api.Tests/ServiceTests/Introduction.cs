using JbHiFi.Weather.Api.Controllers;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

namespace JbHiFi.Weather.Api.Tests.ServiceTests
{
    public class Introduction
    {

        private readonly IOptions<OpenWeatherAPISettings> _testSettings;

        public Introduction()
        {
            _testSettings = Options.Create(new OpenWeatherAPISettings()
            {
                AppId = "8b7535b42fe1c551f18028f64e8688f7",
                BaseUrl = "http://api.openweathermap.org/data/2.5/"
            });

        }


        [Fact]
        public async void IntegrationWithOpenWeatherSuccessful()
        {
            
            var service = new OpenWeatherApiService(_testSettings,new HttpClient());
            
            var result = await service.GetWeather();

            Assert.True(result.Contains("\"name\":\"London\""));
            
        }


        [Fact]
        public async void Service_returns_when_successful()
        {

                var body = "\"name\":\"London\"";
            
                var mockMessageHandler = new Mock<HttpMessageHandler>();
                
                mockMessageHandler.Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(body)
                    });

                
                var underTest = new OpenWeatherApiService(_testSettings,new HttpClient(mockMessageHandler.Object));

                var result = await underTest.GetWeather();
            
                Assert.True(result.Contains("\"name\":\"London\""));

        }
        [Fact]
        public void Service_returns_exception_when_unsuccessful()
        {

            var body = "\"name\":\"London\"";

            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(body)
                });


            var underTest = new OpenWeatherApiService(_testSettings, new HttpClient(mockMessageHandler.Object));

            var exception =  Assert.ThrowsAsync<Exception>(async () => await underTest.GetWeather());

            Assert.True(exception.Result.Message.Contains("failed"));

        }
    }
}
