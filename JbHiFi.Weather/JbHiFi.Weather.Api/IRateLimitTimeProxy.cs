namespace JbHiFi.Weather.Api;

public interface IRateLimitTimeProxy
{
    void SetDateTime(DateTime datetime);
    DateTime GetDateTime();
}