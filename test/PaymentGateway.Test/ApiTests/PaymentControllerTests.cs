using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.API.Controllers;
using PaymentGateway.API.Extensions;
using PaymentGateway.API.Models;
using PaymentGateway.API.Services.Contracts;
using PaymentGateway.Core.Models;
using PaymentGateway.Data.Models;
using PaymentGateway.Data.Repositories.Contracts;
using PaymentGateway.Mock.BankA.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Test.ApiTests
{
    public class PaymentControllerTests
    {
        [Fact]
        public async Task Get_NullPayment_ReturnsNotFound()
        {
            var mockPaymentRepo = new Mock<IPaymentRepository>();
            var mockLogger = new Mock<ILogger<PaymentsController>>();
            var mockMapper = new Mock<IMapper>();
            var mockBankRequestService = new Mock<IBankRequestService>();
            var mockEncryptionService = new Mock<IEncryptionService>();

            mockPaymentRepo.Setup(x => x.GetPaymentAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

            var paymentsController =  new PaymentsController(mockPaymentRepo.Object, mockBankRequestService.Object, mockEncryptionService.Object, mockMapper.Object, mockLogger.Object);
            
            var response = await paymentsController.Get(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task Get_PaymentReturns_ReturnsOkResult()
        {
            var mockPaymentRepo = new Mock<IPaymentRepository>();
            var mockLogger = new Mock<ILogger<PaymentsController>>();
            var mockMapper = new Mock<IMapper>();
            var mockBankRequestService = new Mock<IBankRequestService>();
            var mockEncryptionService = new Mock<IEncryptionService>();

            var mapperResponse = new GetPaymentResponse
            {
                Name = "Fred Bloggs",
                CardNumber = "4111111111111111",
                Id = Guid.NewGuid(),
                PaymentStatus = "Completed"
            };

            mockPaymentRepo.Setup(x => x.GetPaymentAsync(It.IsAny<Guid>())).ReturnsAsync(() => new Payment());
            mockMapper.Setup(x => x.Map<GetPaymentResponse>(It.IsAny<Payment>())).Returns(mapperResponse);
            mockEncryptionService.Setup(x => x.Decrypt(It.IsAny<byte[]>())).Returns("4111111111111111");

            var paymentsController = new PaymentsController(mockPaymentRepo.Object, mockBankRequestService.Object, mockEncryptionService.Object, mockMapper.Object, mockLogger.Object);

            var response = await paymentsController.Get(Guid.NewGuid());

            var okResponse = Assert.IsType<OkObjectResult>(response);
            var getPaymentResponse = Assert.IsType<GetPaymentResponse>(okResponse.Value);
            Assert.Equal(mapperResponse.CardNumber.MaskCard(), getPaymentResponse.CardNumber);
            Assert.Equal(mapperResponse.Name, getPaymentResponse.Name);
            Assert.Equal(mapperResponse.Id, getPaymentResponse.Id);
            Assert.Equal(mapperResponse.PaymentStatus, getPaymentResponse.PaymentStatus);
        }

        [Fact]
        public async Task Post_DatabaseFails_Returns500()
        {
            var mockPaymentRepo = new Mock<IPaymentRepository>();
            var mockLogger = new Mock<ILogger<PaymentsController>>();
            var mockMapper = new Mock<IMapper>();
            var mockBankRequestService = new Mock<IBankRequestService>();
            var mockEncryptionService = new Mock<IEncryptionService>();

            var payment = new Payment { Id = Guid.NewGuid() };
            mockPaymentRepo.Setup(x => x.AddPaymentAsync(It.IsAny<Payment>())).Throws(new DbUpdateException());
            mockMapper.Setup(x => x.Map<Payment>(It.IsAny<PostPaymentRequest>())).Returns(payment);

            var paymentsController = new PaymentsController(mockPaymentRepo.Object, mockBankRequestService.Object, mockEncryptionService.Object, mockMapper.Object, mockLogger.Object);

            var response = await paymentsController.Post(new PostPaymentRequest { CardNumber = "4111111111111111" });

            var statusCodeResult = Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Post_InvalidModel_Returns400()
        {
            var mockPaymentRepo = new Mock<IPaymentRepository>();
            var mockLogger = new Mock<ILogger<PaymentsController>>();
            var mockMapper = new Mock<IMapper>();
            var mockBankRequestService = new Mock<IBankRequestService>();
            var mockEncryptionService = new Mock<IEncryptionService>();

            var paymentsController = new PaymentsController(mockPaymentRepo.Object, mockBankRequestService.Object, mockEncryptionService.Object, mockMapper.Object, mockLogger.Object);
            paymentsController.ModelState.AddModelError("Name", "Required");

            var response = await paymentsController.Post(new PostPaymentRequest { CardNumber = "4111" });

            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public async Task Post_Success_Returns200()
        {
            var mockPaymentRepo = new Mock<IPaymentRepository>();
            var mockLogger = new Mock<ILogger<PaymentsController>>();
            var mockMapper = new Mock<IMapper>();
            var mockBankRequestService = new Mock<IBankRequestService>();
            var mockEncryptionService = new Mock<IEncryptionService>();

            var payment = new Payment { Id = Guid.NewGuid() };
            mockMapper.Setup(x => x.Map<Payment>(It.IsAny<PostPaymentRequest>())).Returns(payment);
            mockBankRequestService.Setup(x => x.PostBankRequestAsync(It.IsAny<BankRequest>())).ReturnsAsync(() => { return new BankResponse(); });

            var paymentsController = new PaymentsController(mockPaymentRepo.Object, mockBankRequestService.Object, mockEncryptionService.Object, mockMapper.Object, mockLogger.Object);

            var response = await paymentsController.Post(new PostPaymentRequest { CardNumber = "4111111111111111" });

            var okResult = Assert.IsType<OkObjectResult>(response);
            var paymentResponse = Assert.IsType<PostPaymentResponse>(okResult.Value);
            Assert.Equal(payment.Id, paymentResponse.PaymentId);
        }
    }
}
