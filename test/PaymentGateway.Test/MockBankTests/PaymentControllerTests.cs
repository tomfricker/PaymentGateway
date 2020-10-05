using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Core.Models;
using PaymentGateway.Mock.BankA.Controller;
using PaymentGateway.Mock.BankA.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PaymentGateway.Test.MockBankTests
{
    public class PaymentControllerTests
    {
        [Fact]
        public void Post_ReturnsBankResponse()
        {
            var paymentController = new PaymentController();

            var response = paymentController.Post(new BankRequest());

            Assert.IsType<BankResponse>(response);
            Assert.InRange((int)response.PaymentStatus, 0, 2);
        }
    }
}
