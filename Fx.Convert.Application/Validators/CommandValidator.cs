using System;
using System.Globalization;
using Fx.Convert.Framework;

namespace Fx.Convert.Application.Validators
{
    public class CommandValidator
    {
        private static readonly StringComparison CommandComparison = StringComparison.OrdinalIgnoreCase;
        private static readonly NumberStyles AmountNumberStyles = NumberStyles.Number;
        private static readonly IFormatProvider AmountFormat = CultureInfo.InvariantCulture;

        public bool Validate(
            string input,
            out string fromCurrency,
            out string toCurrency,
            out decimal amount,
            out string errorMessage)
        {
            fromCurrency = string.Empty;
            toCurrency = string.Empty;
            amount = 0m;
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = ErrorMessages.NoInputProvided;
                return false;
            }

            // Split into at most 3 segments: command, pair, amount
            var parts = input
                .Trim()
                .Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3
             || !parts[0].Equals(CommandMessages.StartCommand, CommandComparison))
            {
                errorMessage = ErrorMessages.InvalidFormat;
                return false;
            }

            // Validate and capture currencies with a single regex match
            var match = RegexPatterns
                .IsoCurrencyExchangePatterns
                .Match(parts[1]);

            if (!match.Success)
            {
                errorMessage = ErrorMessages.InvalidCurrencyFormat;
                return false;
            }

            // Extract and normalize
            fromCurrency = match.Groups[1].Value.ToUpperInvariant();
            toCurrency = match.Groups[2].Value.ToUpperInvariant();

            // Parse amount using invariant culture
            if (!decimal.TryParse(
                    parts[2],
                    AmountNumberStyles,
                    AmountFormat,
                    out amount))
            {
                errorMessage = ErrorMessages.InvalidAmount;
                return false;
            }

            return true;
        }
    }
}
