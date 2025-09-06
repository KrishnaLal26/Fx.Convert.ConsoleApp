namespace Fx.Convert.Domain
{
    public class CurrencyRate
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Rate { get; set; }

        public decimal Convert(decimal amount) => amount * Rate;
    }
}
