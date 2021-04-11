namespace Trainline.CurrencyConverter.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Interface;
    using Models;

    public class CurrencyConversionService: ICurrencyConversionService
    {
        private IExchangeRateService _exchangeRateService;
        private readonly HashSet<string> supportedCurrencies = new HashSet<string>{ "GBP", "EUR", "USD"};

        public CurrencyConversionService(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        public async Task<CurrencyResponse> ConvertAsync(CurrencyRequest currencyRequest)
        {
            if (currencyRequest == null || string.IsNullOrEmpty(currencyRequest.SourceCurrency)
                || string.IsNullOrEmpty(currencyRequest.TargetCurrency))
                return null;

            currencyRequest.SourceCurrency = currencyRequest.SourceCurrency.ToUpper();
            currencyRequest.TargetCurrency = currencyRequest.TargetCurrency.ToUpper();

            if (CheckIfCurrenciesAreSupported(currencyRequest)) return null;

            var response = await _exchangeRateService.GetExchangeRatesAsync(currencyRequest.SourceCurrency);

            if (response == null) return null;

            var targetRate = response.Rates[currencyRequest.TargetCurrency];

            var convertedAmount = currencyRequest.Amount * Math.Round(targetRate, 2);

            return new CurrencyResponse
            {
                ConvertedAmount = convertedAmount,
                TargetCurrency = currencyRequest.TargetCurrency
            };
        }

        private bool CheckIfCurrenciesAreSupported(CurrencyRequest currencyRequest)
        {
            return !supportedCurrencies.Contains(currencyRequest.SourceCurrency) ||
                   !supportedCurrencies.Contains(currencyRequest.TargetCurrency);
        }
    }
}