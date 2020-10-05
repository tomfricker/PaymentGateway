using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.API.Models
{
    public class PostPaymentRequest
    {
        [Required]
        public string Name { get; set; }
               
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string CurrencyCode { get; set; }

        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string ExpiryMonth { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string ExpiryYear { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string CVV { get; set; }
    }
}
