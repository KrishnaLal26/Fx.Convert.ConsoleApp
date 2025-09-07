namespace Fx.Convert.Framework
{
    public static class ErrorMessages
    {
        public const string NoInputProvided = "No input provided.";
        public const string InvalidFormat = "Invalid format. Use: Exchange <Fromcurrency/ToCurrency> <amount>.";
        public const string InvalidCurrencyFormat = "Invalid format. Use ISO codes: AAA/BBB.";
        public const string InvalidAmount = "Invalid amount. Please enter a numeric value.";
        public static string MissingRate(string from, string to) => $"Supplied {from}/{to} exchange rate is missing.";
    }
}
