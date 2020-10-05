using System;
using System.ComponentModel.DataAnnotations;
using PaymentGateway.Core.Enums;

namespace PaymentGateway.Data.Models
{
    public class Payment
    {
        public Guid Id { get; set; }

        public byte[] CardNumber { get; set; }

        public string Name { get; set; }

        public string CVV { get; set; }

        public string ExpiryYear { get; set; }

        public string ExpiryMonth { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public Guid BankResponseId { get; set; }

        public PaymentStatusCode PaymentStatus { get; set; }
    }
}
