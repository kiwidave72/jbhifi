using JbHiFi.Weather.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace JbHiFi.Weather.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AppIdRequirement]
    public class ForecastController : ControllerBase
    {

        private readonly ILogger<ForecastController> _logger;
        private readonly IRateLimiter _rateLimiter;
        private readonly IWeatherService _service;

        public ForecastController(ILogger<ForecastController> logger,
             IRateLimiter rateLimiter , IWeatherService service )
        {
            _logger = logger;
            _rateLimiter = rateLimiter;
            _service = service;
        }


        [HttpGet]
       
        public async Task<ActionResult> GetWeather(string AppId,string city,string country)
        {
            try
            {
                _rateLimiter.RecordRequestForAppId(AppId);
                
                return Ok(await _service.GetWeather(city,country));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(429); 

            }

        }
    }
}