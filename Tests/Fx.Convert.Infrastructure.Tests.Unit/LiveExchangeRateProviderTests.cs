using Fx.Convert.Infrastructure.Providers.LiveExchangeRateProvider;
using Fx.Convert.Infrastructure.Providers.LiveExchangeRateProvider.Models;
using Fx.Convert.Tests.Framework;
using Fx.Convert.Tests.FramewoTrk;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;
using System.Text.Json;

namespace Fx.Convert.Infrastructure.Tests.Unit
{
    public class LiveExchangeRateProviderTests : Test<LiveExchangeRateProvider>
    {
        private const string FromCurrency = "USD";
        private const string ToCurrency = "EUR";
        private const decimal ExpectedRate = 1.25m;

        public LiveExchangeRateProviderTests()
        {
            var config = new ExchangeRateApiConfig
            {
                ApiKey = "test-key",
                GetRateEndpoint = "https://api.example.com/rates?apiKey={apiKey}&currency={currency}"
            };
            Get<IOptions<ExchangeRateApiConfig>>().Value.Returns(config);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsRate_WhenResponseIsValid()
        {
            // Arrange
            var response = new ExchangeRateResponseModel
            {
                ConversionRates = new Dictionary<string, decimal>
                { 
                    { ToCurrency, ExpectedRate }
                }
            };
            var jsonString = JsonSerializer.Serialize(response);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonString)
            };
            var handler = new FakeHttpMessageHandler(httpResponseMessage);
            var httpClient = new HttpClient(handler);
            var httpClientfactory = Get<IHttpClientFactory>();
            httpClientfactory.CreateClient(nameof(LiveExchangeRateProvider)).Returns(httpClient);

            // Act
            var result = await Instance.GetExchangeRateAsync(FromCurrency, ToCurrency);

            // Assert
            Assert.Equal(ExpectedRate, result);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsNull_WhenConversionRatesMissing()
        {
            // Arrange
            var response = new ExchangeRateResponseModel();
            var jsonString = JsonSerializer.Serialize(response);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonString)
            };
            var handler = new FakeHttpMessageHandler(httpResponseMessage);
            var httpClient = new HttpClient(handler);
            var httpClientfactory = Get<IHttpClientFactory>();
            httpClientfactory.CreateClient(nameof(LiveExchangeRateProvider)).Returns(httpClient);

            // Act
            var result = await Instance.GetExchangeRateAsync(FromCurrency, ToCurrency);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsNull_WhenResponseFails()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("")
            };
            var handler = new FakeHttpMessageHandler(httpResponseMessage);
            var httpClient = new HttpClient(handler);
            var httpClientfactory = Get<IHttpClientFactory>();
            httpClientfactory.CreateClient(nameof(LiveExchangeRateProvider)).Returns(httpClient);
            // Act
            var result = await Instance.GetExchangeRateAsync(FromCurrency, ToCurrency);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsNull_OnException()
        {
            // Arrange
            Get<IHttpClientFactory>().CreateClient(Arg.Any<string>())
                  .Throws(new Exception("Network error"));

            // Act
            var result = await Instance.GetExchangeRateAsync(FromCurrency, ToCurrency);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void PriorityOrder_Is_One()
        {
            Assert.Equal(1, Instance.PriorityOrder);
        }
    }
}
