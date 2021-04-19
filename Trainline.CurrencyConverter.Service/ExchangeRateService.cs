namespace Trainline.CurrencyConverter.Service
{
    using Interface;
    using Microsoft.Extensions.Options;
    using Models;
    using Options;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ExchangeRatesServiceOptions _exchangeRatesServiceOptions;

        public ExchangeRateService(IHttpClientFactory clientFactory, IOptions<ExchangeRatesServiceOptions> exchangeRatesServiceOptions)
        {
            _clientFactory = clientFactory;
            _exchangeRatesServiceOptions = exchangeRatesServiceOptions.Value;
        }

        public async Task<CurrencyExchangeResponse> GetExchangeRatesAsync(string sourceCurrency)
        {
            var client = _clientFactory.CreateClient();

            var apiRequest = string.Format(_exchangeRatesServiceOptions.ExchangeRatesApiFeedUrl, sourceCurrency);
            var response = await client.GetAsync(apiRequest);

            if (!response.IsSuccessStatusCode) return null;

            var responseString = await response.Content.ReadAsStringAsync();
                
            var currencyExchangeResponse = JsonSerializer.Deserialize<CurrencyExchangeResponse>(responseString);
                
            return currencyExchangeResponse;
        }
    }
}