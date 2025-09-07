using Fx.Convert.Application.Services.Abstractions;
using Fx.Convert.Application.Validators;
using Fx.Convert.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Fx.Convert.ConsoleApp
{
    public class Program
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
            Console.WriteLine(CommandMessages.StartCommand);
            Console.WriteLine(CommandMessages.InputFormatCommand);
            Console.WriteLine(CommandMessages.ExitFormatCommand);
            StartProcessing(serviceProvider);
        }

        private static void StartProcessing(IServiceProvider serviceProvider)
        {
            var currencyService = serviceProvider.GetRequiredService<ICurrencyService>();
            while (true)
            {
                string input = Console.ReadLine();
                if (input.Trim().Equals(CommandMessages.ExitCommand, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(CommandMessages.ClosingMessage);
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
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }
            }
        }
    }
}