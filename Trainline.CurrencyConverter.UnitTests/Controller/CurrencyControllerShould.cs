namespace Trainline.CurrencyConverter.UnitTests.Controller
{
    using System;
    using System.Threading.Tasks;
    using Api.Controllers;
    using CurrencyConverter.Service.Interface;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Moq;
    using Xunit;

    public class CurrencyControllerShould
    {
        private Mock<ICurrencyConversionService> _currencyConversionServiceMock;
        private CurrencyController controller;

        public CurrencyControllerShould()
        {
            _currencyConversionServiceMock = new Mock<ICurrencyConversionService>();
            controller = new CurrencyController(_currencyConversionServiceMock.Object);
        }

        [Fact]
        public async Task Return200StatusCodeOnSuccessfulCurrencyConversionAsync()
        {
            var currencyRequest = new CurrencyRequest { 
                Amount = 24,
                SourceCurrency = "GBP",
                TargetCurrency = "EUR"
            };

            var expectedCurrencyResponse = new CurrencyResponse
            {
                ConvertedAmount = 26,
                TargetCurrency = "EUR"
            };

            _currencyConversionServiceMock.Setup(x => x.ConvertAsync(currencyRequest))
                .Returns(Task.FromResult(expectedCurrencyResponse));

            var response = await controller.Convert(currencyRequest) as OkObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(expectedCurrencyResponse);
        }

       

        [Fact]
        public async Task Return502StatusCodeOnUnsuccessfulCurrencyConversion()
        {
            var currencyRequest = new CurrencyRequest
            {
                Amount = 24,
                SourceCurrency = "GBP",
                TargetCurrency = "EUR"
            };

            _currencyConversionServiceMock.Setup(x => x.ConvertAsync(currencyRequest))
                .Returns(Task.FromResult((CurrencyResponse)null));

            var response = await controller.Convert(currencyRequest) as StatusCodeResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(502);
        }

        [Fact]
        public async Task Return500StatusCodeOnUnhandledException()
        {
            var currencyRequest = new CurrencyRequest
            {
                Amount = 24,
                SourceCurrency = "GBP",
                TargetCurrency = "EUR"
            };

            _currencyConversionServiceMock.Setup(x => x.ConvertAsync(currencyRequest))
                .Throws<Exception>();

            var response = await controller.Convert(currencyRequest) as StatusCodeResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(500);
        }

    }
}