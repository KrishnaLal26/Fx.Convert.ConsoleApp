using System.Threading.Tasks;

namespace Fx.Convert.Application.Services.Abstractions
{
    public interface ICurrencyService
    {
        Task<decimal> ConvertCurrency(string from, string to, decimal amount);
    }
}
