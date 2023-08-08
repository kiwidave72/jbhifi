using System;
using System.Net;
using JbHiFi.OpenWeather.Client;
using JbHiFi.Weather.Api;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace jbHiFi.OpenWeather.Client.Tests
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
            
            var service = new OpenWeatherClient(_testSettings,new HttpClient());
            var exception =await Record.ExceptionAsync( async () => await service.GetWeather());
            Assert.Null(exception);
        }


        [Fact]
        public async void Service_returns_when_successful()
        {

            var body = "{\r\n  \"coord\": {\r\n    \"lon\": 10.99,\r\n    \"lat\": 44.34\r\n  },\r\n  \"weather\": [\r\n    {\r\n      \"id\": 501,\r\n      \"main\": \"Rain\",\r\n      \"description\": \"clear sky\",\r\n      \"icon\": \"10d\"\r\n    }\r\n  ]}";


            var mockMessageHandler = new Mock<HttpMessageHandler>();
                
                mockMessageHandler.Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(body)
                    });

                
                var underTest = new OpenWeatherClient(_testSettings,new HttpClient(mockMessageHandler.Object));

                var result = await underTest.GetWeather();
            
                Assert.True(result.Weather.First().Description.Contains("clear sky"));

        }
        [Fact]
        public void Service_returns_exception_when_unsuccessful()
        {

            var body = "{\r\n  \"coord\": {\r\n    \"lon\": 10.99,\r\n    \"lat\": 44.34\r\n  },\r\n  \"weather\": [\r\n    {\r\n      \"id\": 501,\r\n      \"main\": \"Rain\",\r\n      \"description\": \"moderate rain\",\r\n      \"icon\": \"10d\"\r\n    }\r\n  ]}";

            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(body)
                });


            var underTest = new OpenWeatherClient(_testSettings, new HttpClient(mockMessageHandler.Object));

            var exception =  Assert.ThrowsAsync<Exception>(async () => await underTest.GetWeather());

            Assert.True(exception.Result.Message.Contains("failed"));

        }
    }
}
