namespace PaymentGateway.API.Models
{
    public class PostPaymentRequest
    {
        public string Name { get; set; }

        public string CardNumber { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public string CVV { get; set; }
    }
}
