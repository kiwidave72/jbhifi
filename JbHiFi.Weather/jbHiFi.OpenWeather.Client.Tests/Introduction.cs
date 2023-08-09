using JbHiFi.OpenWeather.Client;
using JbHiFi.Tests.Common;
using JbHiFi.Weather.Api;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit.Abstractions;

namespace jbHiFi.OpenWeather.Client.Tests
{
    public class Introduction
    {

        private readonly IOptions<OpenWeatherAPISettings> _testSettings;
        private readonly XunitLogger<OpenWeatherClient> _logger;

        public Introduction(ITestOutputHelper output)
        {
            _logger = new XunitLogger<OpenWeatherClient>(output);

            _testSettings = Options.Create(new OpenWeatherAPISettings()
            {
                AppId = "8b7535b42fe1c551f18028f64e8688f7",
                BaseUrl = "http://api.openweathermap.org/data/2.5/"
            });

        }


        [Fact]
        public async void IntegrationWithOpenWeatherSuccessful()
        {
            
            var service = new OpenWeatherClient(_testSettings,new HttpClient(), _logger);
            var exception =await Record.ExceptionAsync( async () => await service.GetWeather("london","uk"));
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

                
                var underTest = new OpenWeatherClient(_testSettings,new HttpClient(mockMessageHandler.Object), _logger);

                var result = await underTest.GetWeather("london","uk");
            
                Assert.True(result.Data.Weather.First().Description.Contains("clear sky"));

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


            var underTest = new OpenWeatherClient(_testSettings, new HttpClient(mockMessageHandler.Object), _logger);

            var exception =  Assert.ThrowsAsync<Exception>(async () => await underTest.GetWeather("london", "uk"));

            Assert.True(exception.Result.Message.Contains("failed"));

        }


        [Fact]
        public void Service_returns_exception_when_ratelimit_is_excedded()
        {

            var body = "{}";

            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.TooManyRequests,
                    Content = new StringContent(body)
                });


            var underTest = new OpenWeatherClient(_testSettings, new HttpClient(mockMessageHandler.Object),_logger);

            var exception = Assert.ThrowsAsync<Exception>(async () => await underTest.GetWeather("london", "uk"));

            Assert.True(exception.Result.Message.Contains("OpenWeather Rate Limit exceeded."));

        }

        [Fact]
        public async void Service_returns_validation_error_when_fields_are_blank()
        {

            var body = "{\"type\":\"https://tools.ietf.org/html/rfc7231#section-6.5.1\",\"title\":\"One or more validation errors occurred.\",\"status\":400,\"traceId\":\"00-53b786c3b6704b891b2aa16871e55af6-68c8c90c313f24a0-00\",\"errors\":{\"city\":[\"The city field is required.\"],\"country\":[\"The country field is required.\"]}}";


            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(body)
                });


            var underTest = new OpenWeatherClient(_testSettings, new HttpClient(mockMessageHandler.Object), _logger);

            var result = await underTest.GetWeather("", "");
            Assert.False(result.IsSuccess);

            Assert.True(result.ErrorMessage.Contains("One or more validation errors occurred"));

        }
    }

  

}
