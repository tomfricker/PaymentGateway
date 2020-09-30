using PaymentGateway.Core.Enums;
using System;

namespace PaymentGateway.Mock.BankA.Models
{
    public class BankResponse
    {
        public Guid Id { get; set; }

        public PaymentStatusCode PaymentStatus { get; set; }
    }
}
