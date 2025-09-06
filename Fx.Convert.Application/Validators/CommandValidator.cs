using System;

namespace Fx.Convert.Application.Validators
{
    public class CommandValidator
    {
        public bool Validate(string input, out string fromCurrency, out string toCurrency, out decimal amount, out string errorMessage)
        {
            fromCurrency = string.Empty;
            toCurrency = string.Empty;
            amount = 0;
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = "No input provided.";
                return false;
            }

            string[] parts = input.Trim().Split(' ');
            if (parts.Length != 3 || !parts[0].Equals("Exchange", StringComparison.OrdinalIgnoreCase))
            {
                errorMessage = "Invalid format. Use: Exchange <fromcurrency/ToCurrency> <amount>";
                return false;
            }

            string[] currencies = parts[1].ToUpper().Split('/');
            if (currencies.Length != 2)
            {
                errorMessage = "Invalid currency format. Use <Fromcurrency/ToCurrency> (e.g., EUR/DKK).";
                return false;
            }

            fromCurrency = currencies[0];
            toCurrency = currencies[1];

            if (!decimal.TryParse(parts[2], out amount))
            {
                errorMessage = "Invalid amount. Please enter a numeric value.";
                return false;
            }

            return true;
        }
    }
}
