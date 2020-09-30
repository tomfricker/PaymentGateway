using System;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Core.Enums;
using PaymentGateway.Core.Models;
using PaymentGateway.Mock.BankA.Models;

namespace PaymentGateway.Mock.BankA.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public BankResponse Post([FromBody] BankRequest request)
        {
            var random = new Random();

            return new BankResponse
            {
                Id = Guid.NewGuid(),
                PaymentStatus = (PaymentStatusCode)random.Next(0, 2)
            };
        }
    }
}
