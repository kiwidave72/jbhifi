
using JbHiFi.OpenWeather.Client;
using JbHiFi.Weather.Api.Controllers;
using JbHiFi.Weather.Api.Service;
using Microsoft.AspNetCore.Authorization;

namespace JbHiFi.Weather.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    builder =>
                    {
                        builder.SetIsOriginAllowed(isOriginAllowed: _ => true) //for all origins
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithExposedHeaders("Content-Disposition")
                            .AllowCredentials();
                    });
            });

            builder.Services.Configure<OpenWeatherAPISettings>(builder.Configuration.GetSection("OpenWeatherAPISettings"));
            builder.Services.Configure<RateLimiterSettings>(builder.Configuration.GetSection("RateLimiterSettings"));
            builder.Services.Configure<AuthenticationSettings>(builder.Configuration.GetSection("AuthenticationSettings"));


            
            builder.Services.AddScoped<IWeatherService, WeatherService>();
            builder.Services.AddScoped<IOpenWeatherClient, OpenWeatherClient>();
            
            builder.Services.AddSingleton<IRateLimitTimeProxy, RateLimitTimeProxy>();
            builder.Services.AddSingleton<IRateLimiter, RateLimiter>();

            builder.Services.AddSingleton<ISlidingWindowValidator, SlidingWindowValidator>();

            builder.Services.AddHttpClient();
            
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            builder.Services.AddHttpContextAccessor();
            
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ClientAppIdPolicy", policy =>
                {
                    policy.Requirements.Add(new AppIdRequirement());
                });
            });

            var app = builder.Build();

            app.UseCors("CorsPolicy");

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