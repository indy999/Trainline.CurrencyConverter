namespace Trainline.CurrencyConverter.Service.Interface
{
    using System.Threading.Tasks;
    using Models;

    public interface ICurrencyConversionService
    {
        Task<CurrencyResponse> ConvertAsync(CurrencyRequest currencyRequest);
    }
}