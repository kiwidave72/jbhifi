using JbHiFi.OpenWeather.Client;
using JbHiFi.Weather.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.RateLimiting;

namespace JbHiFi.Weather.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForecastController : ControllerBase
    {

        private readonly ILogger<ForecastController> _logger;
        private readonly IOpenWeatherClient _openWeatherApi;
        private readonly IRateLimiter _rateLimiter;

        public ForecastController(ILogger<ForecastController> logger,
            IOpenWeatherClient openWeatherApi , IRateLimiter rateLimiter )
        {
            _logger = logger;
            _openWeatherApi = openWeatherApi;
            _rateLimiter = rateLimiter;
        }


        [HttpGet]
        public async Task<ActionResult> GetWeather()
        {
            try
            {
                _rateLimiter.RecordRequestForAppId("x");

                var openWeather = await _openWeatherApi.GetWeather();

                WeatherModel weather = new WeatherModel();

                weather.Description = openWeather.Weather.First().Description;

                return Ok(weather);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(429);

            }

        }
    }
}