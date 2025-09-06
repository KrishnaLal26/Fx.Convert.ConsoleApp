using Fx.Convert.Framework;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fx.Convert.Infrastructure.Providers.LiveExchangeRateProvider.Models;
using Fx.Convert.Domain.Providers;

namespace Fx.Convert.Infrastructure.Providers.LiveExchangeRateProvider
{
    public class LiveExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<ExchangeRateApiConfig> _apiConfig;
        
        public LiveExchangeRateProvider(IHttpClientFactory httpClientFactory, IOptions<ExchangeRateApiConfig> apiConfig)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _apiConfig = apiConfig ?? throw new ArgumentNullException(nameof(apiConfig));
        }

        public int PriorityOrder => 1;

        public async Task<decimal?> GetExchangeRateAsync(string from, string to)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient(nameof(LiveExchangeRateProvider));
                var url = _apiConfig.Value.GetRateEndpoint.InjectParamValues(
                    ("apiKey", _apiConfig.Value.ApiKey),
                    ("currency", from));
                var result = await httpClient.GetAsync<ExchangeRateResponseModel>(url);
                if (!result.IsSuccessStatusCode || result?.Data?.ConversionRates == null)
                {
                    return null;
                }

                result.Data.ConversionRates.TryGetValue(to, out decimal rate);
                return rate;
            }
            catch
            {
                return null;
            }
        }
    }
}
