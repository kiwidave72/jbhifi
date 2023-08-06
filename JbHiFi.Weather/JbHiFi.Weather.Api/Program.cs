
using JbHiFi.Weather.Api.Controllers;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace JbHiFi.Weather.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.Configure<OpenWeatherAPISettings>(builder.Configuration.GetSection("OpenWeatherAPISettings"));

            builder.Services.AddScoped<IOpenWeatherApiService, OpenWeatherApiService>();

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