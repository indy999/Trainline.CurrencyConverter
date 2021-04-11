namespace Trainline.CurrencyConverter.UnitTests.Service
{
    using CurrencyConverter.Service;
    using CurrencyConverter.Service.Interface;
    using FluentAssertions;
    using Models;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class CurrencyConversionServiceShould
    {
        private readonly Mock<IExchangeRateService> _exchangeRateServiceMock;
        private readonly CurrencyConversionService currencyConversionService;

        public CurrencyConversionServiceShould()
        {
            _exchangeRateServiceMock = new Mock<IExchangeRateService>();
            currencyConversionService = new CurrencyConversionService(_exchangeRateServiceMock.Object);
        }

        [Fact]
        public async Task ReturnSuccessfulCurrencyResponseOnConversionForGbpToEur()
        {
            var currencyRequest = new CurrencyRequest
            {
                Amount = 24,
                SourceCurrency = "GBP",
                TargetCurrency = "EUR"
            };

            var currencyExchangeResponse = new CurrencyExchangeResponse
            {
                Base = currencyRequest.SourceCurrency,
                Date = DateTime.Now,
                TimeLastUpdated = DateTime.Now.Ticks,
                Rates = new Dictionary<string, decimal>
                {
                    {"GBP", 1 },
                    {"EUR", 1.168852M},
                    {"USD", 1.384935M}
                }
            };
            _exchangeRateServiceMock.Setup(x => x.GetExchangeRatesAsync(currencyRequest.SourceCurrency))
                .Returns(Task.FromResult(currencyExchangeResponse));

            var response = await currencyConversionService.ConvertAsync(currencyRequest);

            response.Should().NotBeNull();
            response.ConvertedAmount.Should().Be(28.08M);
            response.TargetCurrency.Should().Be("EUR");
        }

        [Fact]
        public async Task ReturnSuccessfulCurrencyResponseOnConversionForEurToGbp()
        {
            var currencyRequest = new CurrencyRequest
            {
                Amount = 24,
                SourceCurrency = "EUR",
                TargetCurrency = "GBP"
            };

            var currencyExchangeResponse = new CurrencyExchangeResponse
            {
                Base = currencyRequest.SourceCurrency,
                Date = DateTime.Now,
                TimeLastUpdated = DateTime.Now.Ticks,
                Rates = new Dictionary<string, decimal>
                {
                    {"GBP", 0.855552M },
                    {"EUR", 1 },
                    {"USD", 1.183894M }
                }
            };
            _exchangeRateServiceMock.Setup(x => x.GetExchangeRatesAsync(currencyRequest.SourceCurrency))
                .Returns(Task.FromResult(currencyExchangeResponse));

            var response = await currencyConversionService.ConvertAsync(currencyRequest);

            response.Should().NotBeNull();
            response.ConvertedAmount.Should().Be(20.64M);
            response.TargetCurrency.Should().Be("GBP");
        }

        [Fact]
        public async Task ReturnSuccessfulCurrencyResponseOnConversionForUsdToEur()
        {
            var currencyRequest = new CurrencyRequest
            {
                Amount = 24,
                SourceCurrency = "USD",
                TargetCurrency = "EUR"
            };

            var currencyExchangeResponse = new CurrencyExchangeResponse
            {
                Base = currencyRequest.SourceCurrency,
                Date = DateTime.Now,
                TimeLastUpdated = DateTime.Now.Ticks,
                Rates = new Dictionary<string, decimal>
                {
                    {"GBP", 0.722077M },
                    {"EUR", 0.844712M},
                    {"USD", 1}
                }
            };
            _exchangeRateServiceMock.Setup(x => x.GetExchangeRatesAsync(currencyRequest.SourceCurrency))
                .Returns(Task.FromResult(currencyExchangeResponse));

            var response = await currencyConversionService.ConvertAsync(currencyRequest);

            response.Should().NotBeNull();
            response.ConvertedAmount.Should().Be(20.16M);
            response.TargetCurrency.Should().Be("EUR");
        }

        [Theory]
        [InlineData("AFN","EUR")]
        [InlineData("GBP", "AFN")]
        [InlineData("CHF", "XXX")]
        public async Task ReturnNullIfUsesAnyUnsupportedCurrency(string sourceCurrency, string targetCurrency)
        {
            var currencyRequest = new CurrencyRequest
            {
                Amount = 24,
                SourceCurrency = sourceCurrency,
                TargetCurrency = targetCurrency
            };

            var response = await currencyConversionService.ConvertAsync(currencyRequest);

            response.Should().BeNull();
        }

        [Fact]
        public async Task ReturnsNullIfExchangeRateGatewayError()
        {
            var currencyRequest = new CurrencyRequest
            {
                Amount = 24,
                SourceCurrency = "GBP",
                TargetCurrency = "EUR"
            };

            _exchangeRateServiceMock.Setup(x => x.GetExchangeRatesAsync(currencyRequest.SourceCurrency))
                .Returns(Task.FromResult((CurrencyExchangeResponse) null));

            var response = await currencyConversionService.ConvertAsync(currencyRequest);

            response.Should().BeNull();
        }

        [Fact]
        public async Task ReturnSuccessfulConversionOnLowerCaseCurrencies()
        {
            var currencyRequest = new CurrencyRequest
            {
                Amount = 24,
                SourceCurrency = "gbp",
                TargetCurrency = "eur"
            };

            var currencyExchangeResponse = new CurrencyExchangeResponse
            {
                Base = currencyRequest.SourceCurrency,
                Date = DateTime.Now,
                TimeLastUpdated = DateTime.Now.Ticks,
                Rates = new Dictionary<string, decimal>
                {
                    {"GBP", 1 },
                    {"EUR", 1.168852M},
                    {"USD", 1.384935M}
                }
            };

            _exchangeRateServiceMock.Setup(x => x.GetExchangeRatesAsync(currencyRequest.SourceCurrency.ToUpper()))
                .Returns(Task.FromResult(currencyExchangeResponse));

            var response = await currencyConversionService.ConvertAsync(currencyRequest);

            response.Should().NotBeNull();
            response.ConvertedAmount.Should().Be(28.08M);
            response.TargetCurrency.Should().Be("EUR");
        }

        [Fact]
        public async Task ReturnNullIfCurrencyRequestIsNull()
        {
            var response = await currencyConversionService.ConvertAsync(null);

            response.Should().BeNull();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("EUR", null)]
        [InlineData(null, "USD")]
        public async Task ReturnNullWhenCurrenciesAreNull(string sourceCurrency, string targetCurrency)
        {
            var currencyRequest = new CurrencyRequest
            {
                Amount = 24,
                SourceCurrency = sourceCurrency,
                TargetCurrency = targetCurrency
            };

            var response = await currencyConversionService.ConvertAsync(currencyRequest);

            response.Should().BeNull();
        }
    }
}
