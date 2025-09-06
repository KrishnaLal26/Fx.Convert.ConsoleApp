using Fx.Convert.Application.Services.Abstractions;
using Fx.Convert.Domain.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Convert.Application.Services
{
    public class CompositeExchangeRateProvider : ICompositeExchangeRateProvider
    {
        private readonly IEnumerable<IExchangeRateProvider> _providers;

        public CompositeExchangeRateProvider(IEnumerable<IExchangeRateProvider> providers)
        {
            _providers = providers ?? throw new System.ArgumentNullException(nameof(providers));
        }

        public async Task<decimal?> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            foreach (var providerService in _providers.OrderBy(provider => provider.PriorityOrder))
            {
                var rate = await providerService.GetExchangeRateAsync(fromCurrency, toCurrency);
                if (rate.HasValue)
                    return rate;
            }

            return null;
        }
    }
}
