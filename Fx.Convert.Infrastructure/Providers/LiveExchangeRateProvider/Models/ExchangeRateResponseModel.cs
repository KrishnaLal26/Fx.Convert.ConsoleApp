using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Fx.Convert.Infrastructure.Providers.LiveExchangeRateProvider.Models
{
    public class ExchangeRateResponseModel
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("conversion_rates")]
        public Dictionary<string, decimal> ConversionRates { get; set; }
    }
}
