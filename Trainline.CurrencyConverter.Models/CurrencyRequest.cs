namespace Trainline.CurrencyConverter.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CurrencyRequest
    {
        [Required]

        public decimal Amount { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{3,3}$")]
        public string SourceCurrency { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{3,3}$")]
        public string TargetCurrency { get; set; }
    }
}