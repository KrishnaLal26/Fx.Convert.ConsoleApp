using Fx.Convert.Application.Services;
using Fx.Convert.Application.Services.Abstractions;
using Fx.Convert.Tests.FramewoTrk;
using NSubstitute;

namespace Fx.Convert.Application.Tests.Unit
{
    public class CurrencyConverterTests : Test<CurrencyConverter>
    {
        private const string FromCurrency = "USD";
        private const string ToCurrency = "EUR";
        private const decimal Amount = 100m;

        [Fact]
        public async Task ConvertCurrency_Returns_Product_WhenRateIsPositive()
        {
            // Arrange
            var provider = Get<ICompositeExchangeRateProvider>();
            provider
                .GetExchangeRateAsync(FromCurrency, ToCurrency)
                .Returns(Task.FromResult<decimal?>(1.5m));

            // Act
            var result = await Instance.ConvertCurrency(FromCurrency, ToCurrency, Amount);

            // Assert
            Assert.Equal(150m, result);
        }

        [Fact]
        public async Task ConvertCurrency_Throws_InvalidDataException_WhenRateIsZero()
        {
            // Arrange
            var provider = Get<ICompositeExchangeRateProvider>();
            provider
                .GetExchangeRateAsync(FromCurrency, ToCurrency)
                .Returns(Task.FromResult<decimal?>(0m));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidDataException>(
                () => Instance.ConvertCurrency(FromCurrency, ToCurrency, Amount));
        }

        [Fact]
        public async Task ConvertCurrency_Throws_InvalidDataException_WhenRateIsNull()
        {
            // Arrange
            var provider = Get<ICompositeExchangeRateProvider>();
            provider
                .GetExchangeRateAsync(FromCurrency, ToCurrency)
                .Returns(Task.FromResult<decimal?>(null));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidDataException>(
                () => Instance.ConvertCurrency(FromCurrency, ToCurrency, Amount));
        }

        [Fact]
        public async Task ConvertCurrency_Calls_Provider_Once_With_Correct_Currencies()
        {
            // Arrange
            var provider = Get<ICompositeExchangeRateProvider>();
            const string from = "GBP";
            const string to = "JPY";

            provider
                .GetExchangeRateAsync(from, to)
                .Returns(Task.FromResult<decimal?>(1.2m));

            // Act
            await Instance.ConvertCurrency(from, to, 50m);

            // Assert
            await provider.Received(1).GetExchangeRateAsync(from, to);
        }
    }
}
