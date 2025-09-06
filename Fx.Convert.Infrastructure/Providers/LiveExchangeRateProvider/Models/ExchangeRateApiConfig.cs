using System;

namespace Fx.Convert.Infrastructure.Providers.LiveExchangeRateProvider.Models
{
    public class ExchangeRateApiConfig
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string GetRateEndpoint { get; set; }
        public int RetryCount { get; set; }
        public TimeSpan TimeoutDuration { get; set; }
        public TimeSpan RetryWaitDuration { get; set; }
    }
}
