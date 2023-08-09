using Microsoft.Extensions.Options;

namespace JbHiFi.Weather.Api.RateLimit;

public class RateLimiter : IRateLimiter
{
    private readonly IOptions<RateLimiterSettings> _settings;

    private Dictionary<string,List<long>> _rates;
        
    private readonly ILogger<RateLimiter> _logger;
        
    private IRateLimitTimeProxy _dateTime;
    private readonly ISlidingWindowValidator _validator;

    public RateLimiter(ILogger<RateLimiter> logger,IOptions<RateLimiterSettings> settings,
        IRateLimitTimeProxy  dateLimitTimeProxy,
        ISlidingWindowValidator validator)
    {
        _dateTime = dateLimitTimeProxy;
        _validator = validator;
        _settings = settings;
        _rates = new Dictionary<string,List<long>>();
        _logger = logger;


    }

    public void RecordRequestForAppId(string appId)
    {
        var appRates = new List<long>();
        _logger.LogDebug("Record Request for {appId}");
        if (_rates.ContainsKey(appId))
        {
            appRates = _rates[appId];
                
        }
        else
        {
            appRates = new List<long>();
            _rates.Add(appId, appRates);
        }

        var timeline = new List<long>();
        var currentRequestTicks = _dateTime.GetDateTime().Ticks;
        timeline.AddRange(appRates);
        timeline.Add(currentRequestTicks);

        if (!_validator.Validate(timeline, _settings.Value.Limit, _settings.Value.Time * 60))
        {
            _logger.LogError("rate limit exceeded");

            throw new RateLimitExceededException("rate limit exceeded");
        }

        appRates.Add(_dateTime.GetDateTime().Ticks);
        _logger.LogDebug("Record Request for {appId} - Passed Validation");

    }
}