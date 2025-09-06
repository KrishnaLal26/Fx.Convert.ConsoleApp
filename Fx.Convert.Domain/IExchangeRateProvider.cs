using System.Threading.Tasks;

namespace Fx.Convert.Domain.Providers
{
    public interface IExchangeRateProvider
    {
        Task<decimal?> GetExchangeRateAsync(string fromCurrency, string toCurrency);

        int PriorityOrder { get; }
    }
}
