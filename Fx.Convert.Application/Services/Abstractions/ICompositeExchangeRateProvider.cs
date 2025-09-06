using System.Threading.Tasks;

namespace Fx.Convert.Application.Services.Abstractions
{
    public interface ICompositeExchangeRateProvider
    {
        Task<decimal?> GetExchangeRateAsync(string fromCurrency, string toCurrency);
    }
}
