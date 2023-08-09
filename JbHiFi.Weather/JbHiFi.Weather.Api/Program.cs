
using JbHiFi.OpenWeather.Client;
using JbHiFi.Weather.Api.Controllers;
using JbHiFi.Weather.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using JbHiFi.Weather.Api.Authentication;
using JbHiFi.Weather.Api.Models;
using JbHiFi.Weather.Api.RateLimit;
using JbHiFi.Weather.Api.Response;

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

            //builder.Services.AddControllers();


            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ValidResultActionFilter>();
            });

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
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {

                        var response = new WeatherResponse<WeatherModel>(null, false, "Internal Server Error.");
                        
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));

                    }
                });
            });
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