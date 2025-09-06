using Fx.Convert.Application.Services.Abstractions;
using Fx.Convert.Application.Validators;
using Fx.Convert.ConsoleApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Setup configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // Setup DI
        var services = ServiceRegistry.RegisterServices(configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Start application
        Console.WriteLine("Exchange");
        Console.WriteLine("Usage: Exchange <currency pair> <amount to exchange>");
        Console.WriteLine("Type 'exit' to quit.");
        StartProcessing(serviceProvider);
    }

    private static void StartProcessing(IServiceProvider serviceProvider)
    {
        var currencyService = serviceProvider.GetRequiredService<ICurrencyService>();
        while (true)
        {
            string input = Console.ReadLine();
            if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Goodbye!");
                break;
            }

            if (new CommandValidator().Validate(input, out string fromCurrency, out string toCurrency, out decimal amount, out string errorMessage))
            {
                try
                {
                    decimal result = currencyService.ConvertCurrency(fromCurrency, toCurrency, amount).GetAwaiter().GetResult();
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unknown Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine(errorMessage);
            }
        }
    }
}