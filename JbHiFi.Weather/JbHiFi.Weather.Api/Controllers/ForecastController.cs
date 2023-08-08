using JbHiFi.OpenWeather.Client;
using JbHiFi.Weather.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace JbHiFi.Weather.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AppIdRequirement]
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
       
        public async Task<ActionResult> GetWeather(string AppId,string city,string country)
        {
            try
            {
                _rateLimiter.RecordRequestForAppId(AppId);

                var openWeather = await _openWeatherApi.GetWeather(city,country);

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