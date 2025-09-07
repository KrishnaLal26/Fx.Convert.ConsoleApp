using Fx.Convert.Application.Validators;
using Fx.Convert.Framework;

namespace Fx.Convert.Application.Tests.Validators
{
    public class CommandValidatorTests
    {
        private readonly CommandValidator _validator = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_NullOrWhitespaceInput_ReturnsFalseWithNoInputMessage(string input)
        {
            // Act
            var isValid = _validator.Validate(input,
                out var from, out var to, out var amount, out var error);

            // Assert
            Assert.False(isValid);
            Assert.Equal(string.Empty, from);
            Assert.Equal(string.Empty, to);
            Assert.Equal(0m, amount);
            Assert.Equal(ErrorMessages.NoInputProvided, error);
        }

        [Theory]
        [InlineData("Exchange")]
        [InlineData("E")]
        [InlineData("Exchange AAABBB")]
        [InlineData("Foo USD/EUR 10")]
        [InlineData("ExchangeUSD/EUR 10")]
        [InlineData("Exchange USD/EUR")]
        public void Validate_InvalidFormat_ReturnsFalseWithFormatError(string input)
        {
            // Act
            var isValid = _validator.Validate(input,
                out var from, out var to, out var amount, out var error);

            // Assert
            Assert.False(isValid);
            Assert.Equal(string.Empty, from);
            Assert.Equal(string.Empty, to);
            Assert.Equal(0m, amount);
            Assert.Equal(
                ErrorMessages.InvalidFormat,
                error);
        }

        [Theory]
        [InlineData("Exchange USDEUR 10")]
        [InlineData("Exchange USD/EUR/GBP 10")]
        [InlineData("exchange USDOLLAR/EUR 10")]
        [InlineData("exchange USD/EURO 10")]
        public void Validate_InvalidCurrencyFormat_ReturnsFalseWithCurrencyError(string input)
        {
            // Act
            var isValid = _validator.Validate(input,
                out var from, out var to, out var amount, out var error);

            // Assert
            Assert.False(isValid);
            Assert.Equal(string.Empty, from);
            Assert.Equal(string.Empty, to);
            Assert.Equal(0m, amount);
            Assert.Equal(
                ErrorMessages.InvalidCurrencyFormat,
                error);
        }

        [Theory]
        [InlineData("Exchange USD/EUR ten")]
        [InlineData("Exchange USD/EUR 1.2.3")]
        public void Validate_NonNumericAmount_ReturnsFalseWithAmountError(string input)
        {
            // Act
            var isValid = _validator.Validate(input,
                out var from, out var to, out var amount, out var error);

            // Assert
            Assert.False(isValid);
            Assert.Equal("USD", from);
            Assert.Equal("EUR", to);
            Assert.Equal(0m, amount);
            Assert.Equal(
                ErrorMessages.InvalidAmount,
                error);
        }

        [Theory]
        [InlineData("Exchange USD/EUR 1")]
        [InlineData("exchange USD/EUR 1")]
        [InlineData("exchange usd/EUR 1")]
        [InlineData("exchange Usd/EUR 1")]
        [InlineData("exchange Usd/Eur 1")]
        public void Validate_ValidInput_ReturnsTrueAndParsesValues(string input)
        {
            // Act
            var isValid = _validator.Validate(input,
                out var from, out var to, out var amount, out var error);

            // Assert
            Assert.True(isValid);
            Assert.Equal("EUR", to.ToUpper());
            Assert.Equal("USD", from.ToUpper());
            Assert.Equal(1m, amount);
            Assert.Equal(string.Empty, error);
        }
    }
}
