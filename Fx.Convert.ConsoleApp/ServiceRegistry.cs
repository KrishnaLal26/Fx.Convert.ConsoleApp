using Microsoft.Extensions.DependencyInjection;
using Fx.Convert.Application;
using Fx.Convert.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Fx.Convert.ConsoleApp
{
    public static class ServiceRegistry
    {
        public static ServiceCollection RegisterServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();
            services.AddSingleton(configuration);

            // Register services
            services.AddApplicationServices();
            services.AddInfrastructureServices(configuration);
            return services;
        }
    }
}
