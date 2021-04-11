namespace Trainline.CurrencyConverter.IntegrationTests
{
    using Api;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Models;
    using Xunit;

    public class CurrencyConverterShould : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly HttpClient Client;

        public CurrencyConverterShould(WebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task SuccessfullyConvertGbpToEur()
        {
            decimal amount = 24;
            var sourceCurrency = "GBP";
            var targetCurrency = "EUR";

            string request = CreateCurrencyConversionRequestUrl(amount, sourceCurrency, targetCurrency);

            var response = await Client.GetAsync(request);

            response.StatusCode.Should().Be(200);

            var currencyResponseJson = await response.Content.ReadAsStringAsync();
            var currencyResponse = JsonSerializer.Deserialize<CurrencyResponse>(currencyResponseJson);

            currencyResponse.TargetCurrency.Should().Be(targetCurrency);
        }
        
        [Theory]
        [InlineData(5, null, null)]
        [InlineData(-1.50, "gbr", "eur")]
        [InlineData(15.3, "gbrr", "eur2")]
        [InlineData(5, "gb", "eur")]
        [InlineData(5, "gbr", "e")]
        [InlineData(15, null, "eur")]
        [InlineData(15, "gbr", null)]
        [InlineData(15, "gbr", "e2w")]
        public async Task Return400OnInvalidCurrencyRequestInput(decimal amount, string sourceCurrency, string targetCurrency)
        {
            string request = CreateCurrencyConversionRequestUrl(amount, sourceCurrency, targetCurrency);

            var response = await Client.GetAsync(request);

            response.StatusCode.Should().Be(400);
        }


        private string CreateCurrencyConversionRequestUrl(decimal amount, string sourceCurrency, string targetCurrency)
        {
            return $"/currency/convert?amount={amount}&sourcecurrency={sourceCurrency}&targetcurrency={targetCurrency}";
        }
    }
}