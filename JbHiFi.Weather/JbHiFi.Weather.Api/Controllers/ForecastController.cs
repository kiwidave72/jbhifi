using JbHiFi.Weather.Api.Authentication;
using JbHiFi.Weather.Api.Models;
using JbHiFi.Weather.Api.RateLimit;
using JbHiFi.Weather.Api.Response;
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
        public async Task<WeatherResponse<WeatherModel>> GetWeather(string AppId,string city,string country)
        {
            _rateLimiter.RecordRequestForAppId(AppId);
            
           return await _service.GetWeather(city, country);
        }
    }
}