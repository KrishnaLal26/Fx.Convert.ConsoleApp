using Fx.Convert.Application.Services.Abstractions;
using System.IO;
using System.Threading.Tasks;

namespace Fx.Convert.Application.Services
{
    public class CurrencyConverter : ICurrencyService
    {
        private readonly ICompositeExchangeRateProvider _rateProvider;

        public CurrencyConverter(ICompositeExchangeRateProvider rateProvider)
        {
            _rateProvider = rateProvider;
        }

        public async Task<decimal> ConvertCurrency(string from, string to, decimal amount)
        {
            var result = await _rateProvider.GetExchangeRateAsync(from, to);
            var rate = result.GetValueOrDefault();
            if(rate == 0)
            {
                throw new InvalidDataException($"Supplied {from}/{to} exchange rate is missing.");
            }
            return amount * rate;
        }
    }
}
