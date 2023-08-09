namespace JbHiFi.Weather.Api.RateLimit;

public class RateLimitTimeProxy : IRateLimitTimeProxy
{

    private DateTime _datetime;

    public RateLimitTimeProxy(DateTime datetime)
    {
        _datetime = datetime;
    }
    
    public RateLimitTimeProxy()
    {
        _datetime = DateTime.Now;
    }


    public void SetDateTime(DateTime datetime)
    {
        _datetime = datetime;
    }
    public DateTime GetDateTime()
    {
        return _datetime ;
    }

}