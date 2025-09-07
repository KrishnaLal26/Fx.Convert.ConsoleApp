using Fx.Convert.Application.Services;
using Fx.Convert.Domain.Providers;
using Fx.Convert.Tests.FramewoTrk;
using NSubstitute;

namespace Fx.Convert.ConsoleApp.Tests.Unit
{
    public class CompositeExchangeRateProviderTests : Test<CompositeExchangeRateProvider>
    {
        [Fact]
        public async Task Returns_First_NonNull_Rate_From_Providers_In_Order()
        {
            // Arrange
            var mockProvider1 = Substitute.For<IExchangeRateProvider>();
            mockProvider1.PriorityOrder.Returns(2);
            mockProvider1.GetExchangeRateAsync("USD", "EUR").Returns((decimal?)null);
            var mockProvider2 = Substitute.For<IExchangeRateProvider>();
            mockProvider2.PriorityOrder.Returns(1);
            mockProvider2.GetExchangeRateAsync("USD", "EUR").Returns(1.23m);
            var providers = new List<IExchangeRateProvider> { mockProvider1, mockProvider2 };
            Get<IEnumerable<IExchangeRateProvider>>().GetEnumerator().Returns(_ => providers.GetEnumerator());

            // Act
            var result = await Instance.GetExchangeRateAsync("USD", "EUR");

            // Assert
            Assert.Equal(1.23m, result);
        }

        [Fact]
        public async Task Returns_Null_If_All_Providers_Return_Null()
        {
            // Arrange
            var mockProvider1 = Substitute.For<IExchangeRateProvider>();
            mockProvider1.PriorityOrder.Returns(2);
            mockProvider1.GetExchangeRateAsync("USD", "EUR").Returns((decimal?)null);
            var mockProvider2 = Substitute.For<IExchangeRateProvider>();
            mockProvider2.PriorityOrder.Returns(1);
            mockProvider2.GetExchangeRateAsync("USD", "EUR").Returns((decimal?)null);
            var providers = new List<IExchangeRateProvider> { mockProvider1, mockProvider2 };
            Get<IEnumerable<IExchangeRateProvider>>().GetEnumerator().Returns(_ => providers.GetEnumerator());

            // Act
            var result = await Instance.GetExchangeRateAsync("USD", "EUR");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Constructor_Throws_If_Providers_Is_Null()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CompositeExchangeRateProvider(null));
        }
    }
}