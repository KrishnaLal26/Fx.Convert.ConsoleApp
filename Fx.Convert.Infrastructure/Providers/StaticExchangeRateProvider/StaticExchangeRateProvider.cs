using Fx.Convert.Domain.Providers;
using Fx.Convert.Infrastructure.Providers.StaticExchangeRateProvider.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Convert.Infrastructure.Providers.StaticExchangeRateProvider
{
    public class StaticExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IOptions<List<ExchangeRateConfigModel>> _config;

        public StaticExchangeRateProvider(IOptions<List<ExchangeRateConfigModel>> config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public int PriorityOrder => 2;

        public Task<decimal?> GetExchangeRateAsync(string from, string to)
        {
            return Task.FromResult(_config.Value.FirstOrDefault(rate => rate.From.Equals(from, StringComparison.OrdinalIgnoreCase)
            && rate.To.Equals(to, StringComparison.OrdinalIgnoreCase))?.Rate);
        }
    }
}
