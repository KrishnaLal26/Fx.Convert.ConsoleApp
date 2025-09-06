using Fx.Convert.Domain.Providers;
using Fx.Convert.Framework;
using Fx.Convert.Infrastructure.Providers.LiveExchangeRateProvider;
using Fx.Convert.Infrastructure.Providers.LiveExchangeRateProvider.Models;
using Fx.Convert.Infrastructure.Providers.StaticExchangeRateProvider;
using Fx.Convert.Infrastructure.Providers.StaticExchangeRateProvider.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Fx.Convert.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddStaticIsoExchangeProvider(services, configuration);
            AddLiveIsoExchangeProvider(services, configuration);
        }

        private static void AddStaticIsoExchangeProvider(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<List<ExchangeRateConfigModel>>(configuration.GetSection("ExchangeRates"));
            services.AddSingleton<IExchangeRateProvider, StaticExchangeRateProvider>();
        }

        private static void AddLiveIsoExchangeProvider(IServiceCollection services, IConfiguration configuration)
        {
            var configSection = configuration.GetSection("ExchangeRateApi");
            services.Configure<ExchangeRateApiConfig>(configSection);
            services.AddSingleton<IExchangeRateProvider, LiveExchangeRateProvider>();
            var config = configSection.Get<ExchangeRateApiConfig>();
            services.AddHttpClient(nameof(LiveExchangeRateProvider),
            client =>
            {
                client.BaseAddress = new Uri(config.BaseUrl);
                client.Timeout = config.TimeoutDuration;
            }).AddPolicyHandler(RetryPolicy.GetRetryPolicy(config.RetryCount, config.RetryWaitDuration));
        }
    }
}
