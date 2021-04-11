namespace Trainline.CurrencyConverter.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class CurrencyExchangeResponse
    {
        [JsonPropertyName("base")]
        public string Base { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("time_last_updated")]
        public long TimeLastUpdated { get; set; }
        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
