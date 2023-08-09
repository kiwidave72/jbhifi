namespace JbHiFi.Weather.Api.RateLimit
{
    public class RateLimitExceededException : Exception
    {
        public RateLimitExceededException(string message) : base(message)
        {

        }
    }
}
