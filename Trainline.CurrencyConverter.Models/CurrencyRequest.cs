namespace Trainline.CurrencyConverter.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CurrencyRequest
    {
        [Required]
        [RegularExpression(@"^\d+\.?\d*$")]
        public decimal Amount { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{3,3}$")]
        public string SourceCurrency { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{3,3}$")]
        public string TargetCurrency { get; set; }
    }
}