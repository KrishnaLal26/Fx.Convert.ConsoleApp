using Fx.Convert.Infrastructure.Providers.StaticExchangeRateProvider;
using Fx.Convert.Infrastructure.Providers.StaticExchangeRateProvider.Models;
using Fx.Convert.Tests.FramewoTrk;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Fx.Convert.Infrastructure.Tests.Unit
{
    public class StaticExchangeRateProviderTests : Test<StaticExchangeRateProvider>
    {
        private const string FromCurrency = "USD";
        private const string ToCurrency = "EUR";

        private void SetupConfig(params ExchangeRateConfigModel[] rates)
        {
            Get<IOptions<List<ExchangeRateConfigModel>>>().Value.Returns(new List<ExchangeRateConfigModel>(rates));
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsRate_WhenPairExists()
        {
            // Arrange
            var config = new ExchangeRateConfigModel
            {
                From = FromCurrency,
                To = ToCurrency,
                Rate = 1.25m
            };
            SetupConfig(config);

            // Act
            var result = await Instance.GetExchangeRateAsync(FromCurrency, ToCurrency);

            // Assert
            Assert.Equal(1.25m, result);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsNull_WhenPairDoesNotExist()
        {
            // Arrange
            var config = new ExchangeRateConfigModel
            {
                From = "GBP",
                To = "JPY",
                Rate = 150.0m
            };
            SetupConfig(config);

            // Act
            var result = await Instance.GetExchangeRateAsync(FromCurrency, ToCurrency);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetExchangeRateAsync_IsCaseInsensitive()
        {
            // Arrange
            var config = new ExchangeRateConfigModel
            {
                From = "usd",
                To = "eur",
                Rate = 0.9m
            };
            SetupConfig(config);

            // Act
            var result = await Instance.GetExchangeRateAsync("USD", "EUR");

            // Assert
            Assert.Equal(0.9m, result);
        }

        [Fact]
        public void PriorityOrder_ReturnsExpectedValue()
        {
            // Act
            var priority = Instance.PriorityOrder;

            // Assert
            Assert.Equal(2, priority);
        }

        [Fact]
        public void Constructor_ThrowsException_WhenConfigIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new StaticExchangeRateProvider(null));

            Assert.Equal("config", ex.ParamName);
        }
    }
}
