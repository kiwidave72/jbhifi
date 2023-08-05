using Microsoft.AspNetCore.Mvc;

namespace JbHiFi.Weather.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForecastController : ControllerBase
    {

        private readonly ILogger<ForecastController> _logger;

        public ForecastController(ILogger<ForecastController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult> GetWeather()
        {
            var client = new HttpClient();

            var response=  await client.GetAsync(
                "http://api.openweathermap.org/data/2.5/weather?q=London,uk&appid=8b7535b42fe1c551f18028f64e8688f7");


            var responseBody = await response.Content.ReadAsStringAsync();


            return Ok(responseBody);

        }
    }
}