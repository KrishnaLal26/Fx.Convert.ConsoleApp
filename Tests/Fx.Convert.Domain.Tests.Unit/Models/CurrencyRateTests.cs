using Fx.Convert.Domain;

namespace Fx.Convert.Domain.Tests.Unit.Models
{
    public class CurrencyRateTests
    {
        [Theory]
        [InlineData(100, 1.5, 150)]
        [InlineData(0, 2.0, 0)]
        [InlineData(50, 0, 0)]
        [InlineData(10, -1.2, -12)]
        public void Convert_ReturnsExpectedResult(decimal amount, decimal rate, decimal expected)
        {
            // Arrange
            var currencyRate = new CurrencyRate
            {
                FromCurrency = "USD",
                ToCurrency = "EUR",
                Rate = rate
            };

            // Act
            var result = currencyRate.Convert(amount);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var currencyRate = new CurrencyRate
            {
                FromCurrency = "GBP",
                ToCurrency = "JPY",
                Rate = 0.0085m
            };

            // Assert
            Assert.Equal("GBP", currencyRate.FromCurrency);
            Assert.Equal("JPY", currencyRate.ToCurrency);
            Assert.Equal(0.0085m, currencyRate.Rate);
        }
    }
}
