namespace Trainline.CurrencyConverter.Models
{
    using System.Text.Json.Serialization;

    public class CurrencyResponse
    {
        [JsonPropertyName("convertedAmount")]
        public decimal ConvertedAmount { get; set; }
        [JsonPropertyName("targetCurrency")]
        public string TargetCurrency { get; set; }
    }
}