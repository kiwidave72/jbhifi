namespace JbHiFi.Weather.Api.RateLimit;

public interface IRateLimiter
{
    void RecordRequestForAppId(string appId);
}