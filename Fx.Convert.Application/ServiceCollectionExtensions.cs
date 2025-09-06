using Fx.Convert.Application.Services;
using Fx.Convert.Application.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fx.Convert.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<ICurrencyService, CurrencyConverter>();
            services.AddSingleton<ICompositeExchangeRateProvider, CompositeExchangeRateProvider>();
        }
    }
}
