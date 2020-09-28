using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.API.Models
{
    public class PaymentRequest
    {
        public string CardNumber { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public string CVV { get; set; }
    }
}
