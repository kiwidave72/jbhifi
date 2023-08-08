namespace JbHiFi.Weather.Api;

public interface ISlidingWindowValidator
{
    bool Validate(List<long> timeline, int windowLimit, int timeInSeconds);
}

public class SlidingWindowValidator : ISlidingWindowValidator
{
    private readonly ILogger<RateLimiter> _logger;

    public SlidingWindowValidator(ILogger<RateLimiter> logger)
    {
        _logger = logger;
    }

    public bool Validate(List<long> timeline, int windowLimit, int timeInSeconds)
    {
        var currentRequestTicks = timeline.OrderDescending().First();

        if (timeline.Any())
        {
            var window = timeline.Where(rate => rate > new DateTime(currentRequestTicks).AddSeconds(-(timeInSeconds)).Ticks).OrderDescending().ToList();

            if (window.Count() > windowLimit)
            {
                _logger.LogDebug($"rate limit exceeded {window.Count()}");
                return false;
            }
                    

        }

        return true;
            
    }
}