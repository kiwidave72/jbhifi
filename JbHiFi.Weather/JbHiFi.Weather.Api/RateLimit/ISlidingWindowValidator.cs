namespace JbHiFi.Weather.Api.RateLimit;

public interface ISlidingWindowValidator
{
    bool Validate(List<long> timeline, int windowLimit, int timeInSeconds);
}