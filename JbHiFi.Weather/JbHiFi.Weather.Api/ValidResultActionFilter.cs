using JbHiFi.Weather.Api.Models;
using JbHiFi.Weather.Api.RateLimit;
using JbHiFi.Weather.Api.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JbHiFi.Weather.Api;

public class ValidResultActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {

        if (context.Exception is RateLimitExceededException)
        {
            var response = new WeatherResponse<WeatherModel>(null, false, "Rate Limit Exceeded");
            context.ExceptionHandled = true;
            context.Result = new BadRequestObjectResult(response);
        }


        if (context.Result is ObjectResult objectResult)
        {
            var value = objectResult.Value;

            if ((value is IWeatherResponse))
            {
                var weatherReponse = value as IWeatherResponse;

                context.Result = new OkObjectResult(weatherReponse);
            }

            else
            {
                context.Result = new BadRequestObjectResult(value);

            }
        }

    }

}