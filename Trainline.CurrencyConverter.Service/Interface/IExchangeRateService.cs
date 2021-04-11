namespace Trainline.CurrencyConverter.Service.Interface
{
    using System.Threading.Tasks;
    using Models;

    public interface IExchangeRateService
    {
        Task<CurrencyExchangeResponse> GetExchangeRatesAsync(string sourceCurrency);
    }
}