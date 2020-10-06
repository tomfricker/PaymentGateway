using PaymentGateway.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.API.Models
{
    public class GetPaymentResponse
    {
        public Guid Id { get; set; }

        public string CardNumber { get; set; }

        public string Name { get; set; }

        public string PaymentStatus { get; set; }
    }
}
