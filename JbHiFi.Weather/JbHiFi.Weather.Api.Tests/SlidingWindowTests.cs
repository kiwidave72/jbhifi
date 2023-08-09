using JbHiFi.Tests.Common;
using JbHiFi.Weather.Api.RateLimit;
using Xunit.Abstractions;

namespace JbHiFi.Weather.Api.Tests
{
    public class SlidingWindowTests
    {
       
        private readonly XunitLogger<RateLimiter> _logger;

        public SlidingWindowTests(ITestOutputHelper output)
        {
            _logger = new XunitLogger<RateLimiter>(output);
        }

        [Theory]
        [InlineData(true, new long[] { 0 })]
        [InlineData(true, new long[] { 0, 0, 0, 0, 0 })] //count
        [InlineData(true, new long[] { 0, 20 })]
        [InlineData(true, new long[] { 0, 20, 30, 40, 55 })] //count in 60
        [InlineData(true, new long[] { 0, 20, 66 })] // count in 60
        [InlineData(true, new long[] { 0, 20, 30, 40, 55, 70, 100, 100, 110, 120, 130 })] //count, boundary of 60 and count 130 - 60 = 70
        [InlineData(false, new long[] { 0, 20, 30, 40, 55, 71, 100, 100, 110, 120, 130 })] // count, boundary of 60 and count 130 - 60 = 70
        [InlineData(false, new long[] { 0, 0, 0, 0, 0, 0 })] // count
        [InlineData(false, new long[] { 0, 20, 20, 20, 20, 20 })] // count
        public void SlidingWindowTest(bool valid, long[] seq)
        {
            var validator = new SlidingWindowValidator(_logger);

            // create timeline where x minutes midnight.
            var timeline = CreateTimeLineFrom(DateTime.Parse("2023/01/01 00:0:00"), seq); // starts x minutes from midnight

            Assert.Equal(valid, validator.Validate(timeline, 5, 60 * 60));

        }

        private List<long> CreateTimeLineFrom(DateTime starTime, long[] seq)
        {
            var timeline = new List<long>();

            foreach (var element in seq)
            {
                timeline.Add(starTime.AddMinutes(element).Ticks);
            }

            return timeline;

        }

    }

}
