using JbHiFi.OpenWeather.Client;
using JbHiFi.Weather.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace JbHiFi.Weather.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForecastController : ControllerBase
    {

        private readonly ILogger<ForecastController> _logger;
        private readonly IOpenWeatherClient _openWeatherApi;

        public ForecastController(ILogger<ForecastController> logger,
            IOpenWeatherClient openWeatherApi)
        {
            _logger = logger;
            _openWeatherApi = openWeatherApi;
        }


        [HttpGet]
        public async Task<ActionResult> GetWeather()
        {
            var openWeather = await _openWeatherApi.GetWeather();

            WeatherModel weather = new WeatherModel();

            weather.Description = openWeather.Weather.First().Description;
            
            return Ok(weather);
        }
    }
}