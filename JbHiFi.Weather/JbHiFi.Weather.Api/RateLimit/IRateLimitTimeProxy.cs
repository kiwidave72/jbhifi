namespace JbHiFi.Weather.Api.RateLimit;

public interface IRateLimitTimeProxy
{
    void SetDateTime(DateTime datetime);
    DateTime GetDateTime();
}