
using JbHiFi.OpenWeather.Client;
using JbHiFi.Weather.Api.Controllers;

namespace JbHiFi.Weather.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.Configure<OpenWeatherAPISettings>(builder.Configuration.GetSection("OpenWeatherAPISettings"));
            builder.Services.Configure<RateLimiterSettings>(builder.Configuration.GetSection("RateLimiterSettings"));


            builder.Services.AddScoped<IOpenWeatherClient, OpenWeatherClient>();
            
            builder.Services.AddSingleton<IRateLimitTimeProxy, RateLimitTimeProxy>();
            builder.Services.AddSingleton<IRateLimiter, RateLimiter>();

            builder.Services.AddSingleton<ISlidingWindowValidator, SlidingWindowValidator>();

            builder.Services.AddHttpClient();
            
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}