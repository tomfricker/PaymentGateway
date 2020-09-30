using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Core.Models
{
    public class BankRequest
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
