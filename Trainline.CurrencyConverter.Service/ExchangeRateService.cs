namespace Trainline.CurrencyConverter.Service
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Interface;
    using Models;

    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _exchangeRateApiRequestUrl = "https://trainlinerecruitment.github.io/exchangerates/api/latest/{0}.json";
        public ExchangeRateService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<CurrencyExchangeResponse> GetExchangeRatesAsync(string sourceCurrency)
        {
            var client = _clientFactory.CreateClient();

            var apiRequest = string.Format(_exchangeRateApiRequestUrl, sourceCurrency);
            var response = await client.GetAsync(apiRequest);

            if (!response.IsSuccessStatusCode) return null;

            var responseString = await response.Content.ReadAsStringAsync();
                
            var currencyExchangeResponse = JsonSerializer.Deserialize<CurrencyExchangeResponse>(responseString);
                
            return currencyExchangeResponse;
        }
    }
}