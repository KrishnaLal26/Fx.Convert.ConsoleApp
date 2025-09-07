using System.Text.RegularExpressions;

namespace Fx.Convert.Framework
{
    public static class RegexPatterns
    {
        // Regex: three letters / three letters
        public static Regex IsoCurrencyExchangePatterns = new Regex(@"^([A-Za-z]{3})/([A-Za-z]{3})$");
    }
}
