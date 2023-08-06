using Microsoft.AspNetCore.Mvc;

namespace JbHiFi.Weather.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForecastController : ControllerBase
    {

        private readonly ILogger<ForecastController> _logger;
        private readonly IOpenWeatherApiService _openWeatherApi;

        public ForecastController(ILogger<ForecastController> logger,
            IOpenWeatherApiService openWeatherApi)
        {
            _logger = logger;
            _openWeatherApi = openWeatherApi;
        }


        [HttpGet]
        public async Task<ActionResult> GetWeather()
        {
            
            return Ok(await _openWeatherApi.GetWeather());
        }
    }
}