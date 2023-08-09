# JB Hi-Fi Weather App Service

### Contrants
The following is considered out of scope for this challenge 
- Database to store Client App IDs is out of scope
- Database to store Rate limits for Client App requests is out of scope and only in memory store is used.
- The react app is very simple for this challenge, and that in production style sheets and Material UI components would be used.
- Project structure could be broken down into more projects and Git repos but have tried to keep things simple.

## Projects
* All projects are c# .net 7. 
* Authentication and Rate Limiting is implemented in this challenge.
* .net 7 contains a new set of classes to manage rate limiting and would be an option for production, also Poly contains a set of classes to do caching/ratelimiting and retrys.
* Also the concept of an API gateway to manage client app authentication (including a AccessToken,RefreshToken) and rate limiting would be a nicer solution and would allow the Weather.Api to just manage the Domain of getting the weather.
* logging is provided but at this time is used for outputting to the unit test runner.

The weather-client react app needs to be started from the commandline (see the readme file in the react folder) while the .net service can be run from visual studio.

For following projects make up the Weather App Web Api Service.

### JbHiFi.OpenWeatherClient
Is a http client wrapper to the Open Weather API.
It manages calls to the Open Weather API and logs errors, and handles responses back to Weather API.

### JbHiFi.Weather.Api
Is a http RestFull API that takes the requests for a client App and get the weather using the OpenWeatherClient.
It handles valid client app Ids using the Authentication framework
And handles the Rate Limiting. (custom coded rate limiting)

### JbHiFi.Weather.Api -  Appsettings.json
All Client App Id are configured in the appsettings.json with the AuthenticationSettings section
* ClientAppId1
* ClientAppId2
* ClientAppId3
* ClientAppId4
* ClientAppId5

RateLimiterSettings section has the Rate and Time settings for the rate limiting of the API.
An RateLimitExceededException will be thrown when the API hits the bounds of the rate limit per client app Id. 

### JbHiFi.Test.Common
A common project for common tests class across the Testing projects.

## Xunit Tests (for integration and unit testing)
There are unit tests for the following

### JbHiFi.OpenWeatherClient.Tests
Unit Tests for the Client with the Mocking of HttpRequestMessage using Moq nuget package.

### JbHiFi.Weather.Api.Test
XUnit integration tests against the Web API are using the Microsoft.AspNetCore.Mvc.Testing nuget package/framework.
XUnit tests for RateLimiter and the SlidingWindowValiator. 

## React Front End
### weather-client
Is a very simple react app that using the .env file for configuration settings for server endpoint and client app id.
The UI is very simple and allows the user to enter the city and country, and to submit the request.
The resulting weather description displayed on and successful response from the server or an error message displayed. (I.E. Rate limit exceeded or validation errors)

