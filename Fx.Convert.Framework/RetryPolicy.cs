using Polly.Extensions.Http;
using Polly;
using System;
using System.Net.Http;

namespace Fx.Convert.Framework
{
    public static class RetryPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount, TimeSpan waitingPeriod)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => waitingPeriod);
        }
    }
}
