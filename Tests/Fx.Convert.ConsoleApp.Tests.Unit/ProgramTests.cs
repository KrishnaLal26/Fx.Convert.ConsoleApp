using Fx.Convert.Application.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Reflection;

namespace Fx.Convert.ConsoleApp.Tests.Unit
{
    public class ProgramTests
    {
        // Grab the private StartProcessing method
        private readonly MethodInfo StartProcessingMethod =
            typeof(Program)
                .GetMethod("StartProcessing", BindingFlags.NonPublic | BindingFlags.Static)!;

        // Helper to build IServiceProvider with a stubbed ICurrencyService
        private IServiceProvider BuildServiceProvider(ICurrencyService currencyService)
        {
            var services = new ServiceCollection();
            services.AddSingleton<ICurrencyService>(currencyService);
            return services.BuildServiceProvider();
        }

        [Fact]
        public void ValidCommand_WritesConversionResult()
        {
            // Arrange
            const string from = "USD";
            const string to = "EUR";
            const decimal amount = 3.14m;
            const decimal expected = 2.681560m;

            var inputBuilder = new StringWriter();
            inputBuilder.WriteLine($"Exchange {from}/{to} {amount}");
            inputBuilder.WriteLine("exit");
            Console.SetIn(new StringReader(inputBuilder.ToString()));
            var output = new StringWriter();
            Console.SetOut(output);

            var currencyService = Substitute.For<ICurrencyService>();
            currencyService
                .ConvertCurrency(from, to, amount)
                .Returns(Task.FromResult(expected));

            var provider = BuildServiceProvider(currencyService);

            // Act
            StartProcessingMethod.Invoke(null, new object[] { provider });

            // Assert
            var consoleOutput = output.ToString();
            Assert.Contains($"{expected}", consoleOutput);
        }

        [Fact]
        public void ConversionThrowsException_WritesUnknownError_ThenGoodbye()
        {
            // Arrange
            const string from = "GBP";
            const string to = "JPY";
            const decimal amount = 1m;
            var exception = new Exception("service down");

            var inputBuilder = new StringWriter();
            inputBuilder.WriteLine($"Exchange {from}/{to} {amount}");
            inputBuilder.WriteLine("exit");

            Console.SetIn(new StringReader(inputBuilder.ToString()));
            var output = new StringWriter();
            Console.SetOut(output);

            var currencyService = Substitute.For<ICurrencyService>();
            currencyService
                .ConvertCurrency(from, to, amount)
                .Throws(exception);

            var provider = BuildServiceProvider(currencyService);

            // Act
            StartProcessingMethod.Invoke(null, new object[] { provider });

            // Assert
            var consoleOutput = output.ToString();
            Assert.Contains($"{exception.Message}", consoleOutput);
        }

    }
}
