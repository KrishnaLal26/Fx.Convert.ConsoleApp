namespace Fx.Convert.Infrastructure.Providers.StaticExchangeRateProvider.Models
{
    public class ExchangeRateConfigModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Rate { get; set; }
    }
}
